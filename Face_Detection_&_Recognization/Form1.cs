using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Face_Detection___Recognization
{
    public partial class Form1 : Form
    {
        MCvFont font = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_TRIPLEX, 0.6d, 0.6d);
        HaarCascade faceDetected;
        Capture camera;
        Image<Bgr, Byte> Frame;
        Image<Gray, Byte> result;
        Image<Gray, Byte> TrainedFace = null;
        Image<Gray, Byte> grayFace = null;
        List<Image<Gray, Byte>> trainingImages = new List<Image<Gray, Byte>>();
        List<string> labels = new List<string>();
        int Count, NumLabeles;

        public Form1()
        {
            InitializeComponent();
            faceDetected = new HaarCascade("haarcascade_frontalface_default.xml");

            // Load the existing database if it exists
            try
            {
                string Labelsinf = File.ReadAllText(Application.StartupPath + "/Faces/Faces.txt");
                string[] Labels = Labelsinf.Split(',');
                NumLabeles = Convert.ToInt16(Labels[0]);
                Count = NumLabeles;

                // Load face images and their labels
                for (int i = 1; i <= NumLabeles; i++)
                {
                    string faceFile = Application.StartupPath + "/Faces/face" + i + ".bmp";
                    trainingImages.Add(new Image<Gray, Byte>(faceFile));
                    labels.Add(Labels[i]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No faces in the database.");
            }
        }

        // Start the camera when the button is clicked
        private void button1_Click(object sender, EventArgs e)
        {
            camera = new Capture();
            camera.QueryFrame();
            Application.Idle += new EventHandler(FrameProcedure);
        }

        // Save a new face image and its label
        private void saveButton_Click(object sender, EventArgs e)
        {
            Count++;
            grayFace = camera.QueryGrayFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            MCvAvgComp[][] detectedFaces = grayFace.DetectHaarCascade(faceDetected, 1.2, 10, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(20, 20));

            foreach (MCvAvgComp f in detectedFaces[0])
            {
                TrainedFace = Frame.Copy(f.rect).Convert<Gray, Byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                break;
            }

            // Save the new face
            trainingImages.Add(TrainedFace);
            labels.Add(textName.Text);
            File.WriteAllText(Application.StartupPath + "/Faces/Faces.txt", trainingImages.Count.ToString() + ",");

            for (int i = 0; i < trainingImages.Count; i++)
            {
                string filePath = Application.StartupPath + "/Faces/face" + (i + 1) + ".bmp";
                trainingImages[i].Save(filePath);
                File.AppendAllText(Application.StartupPath + "/Faces/Faces.txt", labels[i] + ",");
            }

            MessageBox.Show(textName.Text + " added successfully.");
        }

        // Recognize faces from the camera feed
        private void FrameProcedure(object sender, EventArgs e)
        {
            Frame = camera.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            grayFace = Frame.Convert<Gray, Byte>();
            MCvAvgComp[][] facesDetectedNow = grayFace.DetectHaarCascade(faceDetected, 1.2, 10, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(20, 20));

            foreach (MCvAvgComp f in facesDetectedNow[0])
            {
                result = Frame.Copy(f.rect).Convert<Gray, Byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                Frame.Draw(f.rect, new Bgr(Color.Green), 3);

                // Perform face recognition if we have training images
                if (trainingImages.Count > 0)
                {
                    MCvTermCriteria termCriterias = new MCvTermCriteria(Count, 0.001);
                    EigenObjectRecognizer recognizer = new EigenObjectRecognizer(trainingImages.ToArray(), labels.ToArray(), 1500, ref termCriterias);
                    string name = recognizer.Recognize(result);
                    Frame.Draw(name, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.Red));
                }
            }

            cameraBox.Image = Frame;
        }
    }
}
