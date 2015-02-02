# Leaf-Recognition
Leaf Recognition on aforge.net framework

#region COMPONENTS
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Threading;
using System.Linq;

using AForge;
using AForge.Neuro;
using AForge.Neuro.Learning;
using AForge.Controls;
using System.Collections.Generic;
using System.Globalization;
using AForge.Imaging.Filters;
using AForge.Math.Geometry;

#endregion

#region PROGRAM
namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        
        #region GLOBAL VARIABLES
        ActivationNetwork network = null;
        private const int INPUT_SAYISI = 4;
        private const int OUTPUT_SAYISI = 10;
        private const string _DATA = "C:\\Users\\OSMAN\\documents\\visual studio 2013\\Projects\\WindowsFormsApplication2\\WindowsFormsApplication2\\data.txt";
        private const string _DATA2 = "C:\\Users\\OSMAN\\documents\\visual studio 2013\\Projects\\WindowsFormsApplication2\\WindowsFormsApplication2\\data2.txt";
        private const string _DATA5 = "C:\\Users\\OSMAN\\documents\\visual studio 2013\\Projects\\WindowsFormsApplication2\\WindowsFormsApplication2\\data5.txt";

        private const string _DATA6 = "C:\\Users\\OSMAN\\documents\\visual studio 2013\\Projects\\WindowsFormsApplication2\\WindowsFormsApplication2\\deneme1234.txt";

        private const string _DATAM1 = "C:\\Users\\OSMAN\\documents\\visual studio 2013\\Projects\\WindowsFormsApplication2\\WindowsFormsApplication2\\ABC.txt";

        private const string _DATAM2 = "C:\\Users\\OSMAN\\documents\\visual studio 2013\\Projects\\WindowsFormsApplication2\\WindowsFormsApplication2\\ABD.txt";

        private const string _DATAM3 = "C:\\Users\\OSMAN\\documents\\visual studio 2013\\Projects\\WindowsFormsApplication2\\WindowsFormsApplication2\\BCD.txt";

        private const string _DATAM4 = "C:\\Users\\OSMAN\\documents\\visual studio 2013\\Projects\\WindowsFormsApplication2\\WindowsFormsApplication2\\ACD.txt";

       

        public double[][] _inputData = null;
        public double[][] _outputData = null;
        bool dongu = false;
        Image image;
        OpenFileDialog dlg;

        AForge.Imaging.BlobCounter blobCounter = null;

        List<IntPoint> edgePoints;
        List<IntPoint> corners;
        
        int objeSayisi = 100;

        int dosyaSayisi = 1;

        Bitmap _imageGrayScale;
        Bitmap _imageHistogram;
        Bitmap _imageBinary;
        Bitmap _imageIteratif;
        Bitmap _imageSobel;
        Bitmap _imageOtsu;
        Bitmap _imageContrast;
        Bitmap _imageInvert;
        Bitmap _imageErosion;
        Bitmap _imageBinaryErosion;
        Bitmap _imageDilation;
        Bitmap _imageOpening;
        Bitmap _imageClosing;
        Bitmap _imageBiggest;
        Bitmap _imageAccept;

        
        double _yaprakCevre = 0.0;
        double _yaprakEni = 0.0;
        double _yaprakBoyu = 0.0;
        double _yaprakAlanCevreOran = 0.0;
        double _yaprakAlan = 0.0;
        double _yaprakEnBoyOran = 0.0;
        double _fullness = 0.0;
        double _yaprakCevreKenarOran = 0.0;
        #endregion
        
        #region INIT
        public Form1()
        {
            InitializeComponent();
            label3.Text = "Gizli Katman: 20";
            label2.Text = "İterasyon: 1000";
            label4.Text = "Learning Rate: 0.5";
            btn_yaprakSec.Enabled = false;
            btn_findPlant.Enabled = false;

            
        }
        #endregion

        #region LOAD DATA
        // data.txt dosyasından verileri okuyup _inputData dizisine atıyor
        private void button1_Click(object sender, EventArgs e)
        {
            
            List<double[]> inputs = new List<double[]>();
            List<double[]> outputs = new List<double[]>();
            
           
                    #region DataOkuma
                    using ( StreamReader okuyucu = File.OpenText( _DATA5 ) )  // 10 BİTKİ
                    {

                        okuyucu.ReadLine();

                        while ( !okuyucu.EndOfStream )
                        {
                            string satir = okuyucu.ReadLine();
                            string[] bolum = satir.Split( ';' );

                            double[] input = new double[INPUT_SAYISI];
                            double[] output = new double[OUTPUT_SAYISI];

                            //Get data
                            for ( int i = 0; i < INPUT_SAYISI; i++ )
                            {

                               
                                input[i] = double.Parse( bolum[i + 1], NumberFormatInfo.InvariantInfo );
                               
                                if ( i == 0 )
                                {
                                    #region OUTPUTS DECLARETING
                                    if ( bolum[i].ToString() == "1" )
                                    {
                                        output[i + 0] = 1.0;
                                        output[i + 1] = -1.0;
                                        output[i + 2] = -1.0;
                                        output[i + 3] = -1.0;
                                        output[i + 4] = -1.0;
                                        output[i + 5] = -1.0;
                                        output[i + 6] = -1.0;
                                        output[i + 7] = -1.0;
                                        output[i + 8] = -1.0;
                                        output[i + 9] = -1.0;
                                        
                                    }
                                    else if ( bolum[i].ToString() == "2" )
                                    {
                                        output[i + 0] = -1.0;
                                        output[i + 1] = 1.0;
                                        output[i + 2] = -1.0;
                                        output[i + 3] = -1.0;
                                        output[i + 4] = -1.0;
                                        output[i + 5] = -1.0;
                                        output[i + 6] = -1.0;
                                        output[i + 7] = -1.0;
                                        output[i + 8] = -1.0;
                                        output[i + 9] = -1.0;
                                        
                                    }
                                    else if ( bolum[i].ToString() == "3" )
                                    {
                                        output[i + 0] = -1.0;
                                        output[i + 1] = -1.0;
                                        output[i + 2] = 1.0;
                                        output[i + 3] = -1.0;
                                        output[i + 4] = -1.0;
                                        output[i + 5] = -1.0;
                                        output[i + 6] = -1.0;
                                        output[i + 7] = -1.0;
                                        output[i + 8] = -1.0;
                                        output[i + 9] = -1.0;
                                        
                                    }
                                    else if ( bolum[i].ToString() == "4" )
                                    {
                                        output[i + 0] = -1.0;
                                        output[i + 1] = -1.0;
                                        output[i + 2] = -1.0;
                                        output[i + 3] = 1.0;
                                        output[i + 4] = -1.0;
                                        output[i + 5] = -1.0;
                                        output[i + 6] = -1.0;
                                        output[i + 7] = -1.0;
                                        output[i + 8] = -1.0;
                                        output[i + 9] = -1.0;
                                        
                                    }
                                    else if ( bolum[i].ToString() == "5" )
                                    {
                                        output[i + 0] = -1.0;
                                        output[i + 1] = -1.0;
                                        output[i + 2] = -1.0;
                                        output[i + 3] = -1.0;
                                        output[i + 4] = 1.0;
                                        output[i + 5] = -1.0;
                                        output[i + 6] = -1.0;
                                        output[i + 7] = -1.0;
                                        output[i + 8] = -1.0;
                                        output[i + 9] = -1.0;
                                       
                                    }
                                    else if ( bolum[i].ToString() == "6" )
                                    {
                                        output[i + 0] = -1.0;
                                        output[i + 1] = -1.0;
                                        output[i + 2] = -1.0;
                                        output[i + 3] = -1.0;
                                        output[i + 4] = -1.0;
                                        output[i + 5] = 1.0;
                                        output[i + 6] = -1.0;
                                        output[i + 7] = -1.0;
                                        output[i + 8] = -1.0;
                                        output[i + 9] = -1.0;
                                       
                                    }
                                    else if ( bolum[i].ToString() == "7" )
                                    {
                                        output[i + 0] = -1.0;
                                        output[i + 1] = -1.0;
                                        output[i + 2] = -1.0;
                                        output[i + 3] = -1.0;
                                        output[i + 4] = -1.0;
                                        output[i + 5] = -1.0;
                                        output[i + 6] = 1.0;
                                        output[i + 7] = -1.0;
                                        output[i + 8] = -1.0;
                                        output[i + 9] = -1.0;
                                        
                                    }
                                    else if ( bolum[i].ToString() == "8" )
                                    {
                                        output[i + 0] = -1.0;
                                        output[i + 1] = -1.0;
                                        output[i + 2] = -1.0;
                                        output[i + 3] = -1.0;
                                        output[i + 4] = -1.0;
                                        output[i + 5] = -1.0;
                                        output[i + 6] = -1.0;
                                        output[i + 7] = 1.0;
                                        output[i + 8] = -1.0;
                                        output[i + 9] = -1.0;
                                        
                                    }
                                    else if ( bolum[i].ToString() == "9" )
                                    {
                                        output[i + 0] = -1.0;
                                        output[i + 1] = -1.0;
                                        output[i + 2] = -1.0;
                                        output[i + 3] = -1.0;
                                        output[i + 4] = -1.0;
                                        output[i + 5] = -1.0;
                                        output[i + 6] = -1.0;
                                        output[i + 7] = -1.0;
                                        output[i + 8] = 1.0;
                                        output[i + 9] = -1.0;
                                        
                                    }
                                    else if ( bolum[i].ToString() == "10" )
                                    {
                                        output[i + 0] = -1.0;
                                        output[i + 1] = -1.0;
                                        output[i + 2] = -1.0;
                                        output[i + 3] = -1.0;
                                        output[i + 4] = -1.0;
                                        output[i + 5] = -1.0;
                                        output[i + 6] = -1.0;
                                        output[i + 7] = -1.0;
                                        output[i + 8] = -1.0;
                                        output[i + 9] = 1.0;
                                        
                                    }
                                    
                                    
                                    #endregion
                                }
                            }
                            outputs.Add( output );
                            inputs.Add( input );
                        }

                    }
                    #endregion
               


            
            _inputData = inputs.ToArray();
            _outputData = outputs.ToArray();

            //StreamWriter yaz10;
            //for ( int i = 0; i < this._inputData.Length; i++ )
            //{
            //    yaz10 = File.AppendText( "C:\\Users\\OSMAN\\documents\\visual studio 2013\\Projects\\WindowsFormsApplication2\\WindowsFormsApplication2\\1.txt" );
            //    yaz10.WriteLine(
            //       _inputData[i][2] + ";"
                    
            //        );
            //    yaz10.Close();

            //}


            
            //daha sonra try-catch koyulacak
            MessageBox.Show("Plants Data Loaded");
            btn_trainIris.Enabled = true;
            btn_yaprakSec.Enabled = true;

            normalizeEt1( _inputData ); // normalize first inputData (compactness) [0,1]
            normalizeEt2( _inputData ); // normalize second inputData (enBoyOran) [0,1]
            normalizeEt3( _inputData ); // normalize third inputData (fullness) [0,1]
            normalizeEt4( _inputData );


            //StreamWriter yaz1;
            //yaz1 = File.AppendText( "C:\\Users\\OSMAN\\documents\\visual studio 2013\\Projects\\WindowsFormsApplication2\\WindowsFormsApplication2\\dataNormalize.txt" );

            //for ( int i = 0; i < _inputData.Length; i++ )
            //{
            //    yaz1.WriteLine( _inputData[i][0] + ";" + _inputData[i][1] + ";" + _inputData[i][2] );
                

            //}
            //yaz1.Close();
            ////normalizedInputDataWriteToFile( _inputData );
            ////MessageBox.Show("Dönüşümler Başarılı");
            
       }

        

        private void normalizedInputDataWriteToFile( double[][] _inputData )
        {
            StreamWriter yaz2;

            for(int i=0; i < this._inputData.Length; i++)
            {
                yaz2 = File.AppendText( "C:\\Users\\OSMAN\\documents\\visual studio 2013\\Projects\\WindowsFormsApplication2\\WindowsFormsApplication2\\normalizedInputsData.txt" );
                yaz2.WriteLine( 
                    this._inputData[i][0] + ";" + 
                    this._inputData[i][1] + ";" + 
                    this._inputData[i][2]  
                    );
                yaz2.Close();
  
            }

            
            
        }

        
        private void normalizeEt1( double[][] _inputData )
        {
            double max = 0.0;
            double min = this._inputData[0][0];

            for ( int i = 0; i < this._inputData.Length; i++ )
            {
                if ( max < this._inputData[i][0] )
                {
                    max = this._inputData[i][0];
                }
                
            }
            for ( int i = 0; i < this._inputData.Length; i++ )
            {
                if ( this._inputData[i][0] < min )
                {
                    min = this._inputData[i][0];
                }
            }

            //MessageBox.Show( "min: " + min + "\n" + "max: " + max );

            for(int i=0;i<this._inputData.Length;i++)
            {
                this._inputData[i][0] = (this._inputData[i][0] - min) / ( max - min );
                this._inputData[i][0] = Math.Round( this._inputData[i][0], 4 );
            }

            
        }

        private void normalizeEt2( double[][] _inputData )
        {
            double max = 0.0;
            double min = this._inputData[0][1];

            for ( int i = 0; i < this._inputData.Length; i++ )
            {
                if ( max < this._inputData[i][1] )
                {
                    max = this._inputData[i][1];
                }

            }
            for ( int i = 0; i < this._inputData.Length; i++ )
            {
                if ( this._inputData[i][1] < min )
                {
                    min = this._inputData[i][1];
                }
            }

            //MessageBox.Show( "min: " + min + "\n" + "max: " + max );

            for ( int i = 0; i < this._inputData.Length; i++ )
            {
                this._inputData[i][1] = (this._inputData[i][1] - min) / ( max - min );
                this._inputData[i][1] = Math.Round( this._inputData[i][1], 4 );
            }

        }
        
        private void normalizeEt3( double[][] _inputData )
        {
            double max = 0.0;
            double min = this._inputData[0][2];

            for ( int i = 0; i < this._inputData.Length; i++ )
            {
                if ( max < this._inputData[i][2] )
                {
                    max = this._inputData[i][2];
                }

            }
            for ( int i = 0; i < this._inputData.Length; i++ )
            {
                if ( this._inputData[i][2] < min )
                {
                    min = this._inputData[i][2];
                }
            }

            //MessageBox.Show( "min: " + min + "\n" + "max: " + max );

            for ( int i = 0; i < this._inputData.Length; i++ )
            {
                this._inputData[i][2] = (this._inputData[i][2] - min) / ( max - min );
                this._inputData[i][2] = Math.Round( this._inputData[i][2], 4 );
            }

        }

        private void normalizeEt4( double[][] _inputData )
        {
            double max = 0.0;
            double min = this._inputData[0][3];

            for ( int i = 0; i < this._inputData.Length; i++ )
            {
                if ( max < this._inputData[i][3] )
                {
                    max = this._inputData[i][3];
                }

            }
            for ( int i = 0; i < this._inputData.Length; i++ )
            {
                if ( this._inputData[i][3] < min )
                {
                    min = this._inputData[i][3];
                }
            }

            //MessageBox.Show( "min: " + min + "\n" + "max: " + max );

            for ( int i = 0; i < this._inputData.Length; i++ )
            {
                this._inputData[i][3] = ( this._inputData[i][3] - min ) / ( max - min );
                this._inputData[i][3] = Math.Round( this._inputData[i][3], 4 );
            }
        }
        #endregion

        #region TRAIN IRIS DATA
        private void btn_trainIris_Click(object sender, EventArgs e)
        {

            trainData(_inputData, _outputData);
            
            network.Save( "C:\\Users\\OSMAN\\documents\\visual studio 2013\\Projects\\WindowsFormsApplication2\\WindowsFormsApplication2\\my_nn.bin" );
            MessageBox.Show( "Trained Successful " );
            btn_yaprakSec.Enabled = true;

        }

        private void trainData( double[][] _inputData, double[][] _outputData )
        {
            label3.Text = "Gizli Katman: " + trackBar2.Value.ToString();
            label2.Text = "İterasyon: " + trackBar3.Value.ToString();
            label4.Text = "Learning Rate: " + ( trackBar4.Value / 1000.0 ).ToString();

            progressBar1.Maximum = trackBar3.Value;

            
            ////Create Network
            //network = new ActivationNetwork(
            //    new BipolarSigmoidFunction(2), //activation func.
            //    3,                             //input count
            //    trackBar2.Value,               //hidden layer count - değiştirilebilir
            //    10 );                          //output count     

            network = new ActivationNetwork( new BipolarSigmoidFunction( 2 ), 4, 4, 10);
            network.Randomize();



            //Learning Network
            BackPropagationLearning backprob = new BackPropagationLearning( network );
            double deger = trackBar4.Value / 1000.0;
            backprob.LearningRate = deger;
            backprob.Momentum = 0.0;

            int iteration = 1;
            double hata = 0.0;
            //loop
            while ( !dongu )
            {
                hata = backprob.RunEpoch( this._inputData, this._outputData );


                Clipboard.SetText( hata.ToString(), TextDataFormat.Text );
                textBox1.Text = Clipboard.GetText( TextDataFormat.Text );
                Clipboard.SetText( iteration.ToString(), TextDataFormat.Text );
                textBox2.Text = Clipboard.GetText( TextDataFormat.Text );

                progressBar1.Value = iteration;
                iteration++;

                if ( iteration > trackBar3.Value )
                    break;

            }

           

        }

       
        #endregion

        // VISIBLE FALSE
        #region OUTPUT
        private void button1_Click_1(object sender, EventArgs e)
        {
            
            double a1 = Convert.ToDouble(textBox3.Text);
            double a2 = Convert.ToDouble(textBox4.Text);
            double a3 = Convert.ToDouble(textBox5.Text);
            double a4 = Convert.ToDouble(textBox6.Text);

            ////burada aslında 100'e bölme yapıyor 1000'e değil
            double[] girdi = { a1/1000, a2/1000, a3/1000, a4/1000 };

            
            textBox1.Text = network.Compute(girdi)[0].ToString() + " compute"; //output ile aynı değer geliyor
            
            double find = network.Output[0]*100; //denormalize

            //textBox2.Text = (network.Output[0]).ToString() + " output"; //compute ile aynı değer geliyor
            
            //tam sayıya yuvarlama
            find = Math.Round(find,1);


            if (find >= 0.0 && find <= 1.7) //bu aralık optimize edilebilir
            { 
                //MessageBox.Show("Iris-setosa");
                textBox7.Text = "1";
            }

            else if (find > 1.7 && find <= 2.6) 
            { 
                //MessageBox.Show("Iris-versicolor");
                textBox7.Text = "2";
            } 

            else if (find > 2.6 && find <= 3.7) 
            { 
                //MessageBox.Show("Iris-virginica"); 
                textBox7.Text = "3";
            }
            
            else MessageBox.Show("Aralık Dışında");

            
            
        }
        #endregion  
        /////////////////
        public void button2_Click(object sender, EventArgs e)
        {
            btn_binaryTH.Enabled = false;
            using (dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    image = Image.FromFile(dlg.FileName);
                    pictureBox1.Image = image;
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

                    
                }
            }

            btn_binaryTH.Enabled = true;
        }

        public void btn_binaryTH_Click(object sender, EventArgs e)
        {
            
            

            binaryAl();
            
            
            objeSay();

            

            ozellikCikart();


            cevreCiz();

           // enBuyukParcaAl();
            
            #region DENEME
            //// create filter
            //HSLFiltering filter = new HSLFiltering();
            //// set color ranges to keep
            //filter.Hue = new IntRange(70, 140);
            //filter.Saturation = new Range(0.2f, 1);
            //filter.Luminance = new Range(0.1f, 1);
            //// apply the filter
            //imageAccept = filter.Apply(bmp);
            //pictureBox2.Image = imageAccept;
            //pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;

            ////GRAYIMAGE
            //imageGrayScale = grayFilter.Apply(bmp);
            //pictureBox2.Image = imageGrayScale;
            //pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;


            ////HISTOGRAM EQUALIZATION
            //imageHistogram = histogramEqualizationFilter.Apply(imageGrayScale);
            //pictureBox2.Image = imageHistogram;
            //pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;

            //////ITERATIF THRESHOLD
            ////imageBinary = iteratifThFilter.Apply(imageHistogram);
            ////pictureBox2.Image = imageBinary;
            ////pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;

            //////IMAGE CONTRAST
            ////imageContrast = contrastFilter.Apply(imageHistogram);
            ////pictureBox2.Image = imageContrast;
            ////pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;


            ////imageBinary = bestBinaryThFilter.Apply(imageGrayScale);

            ////IMAGE INVERT
            //imageInvert = invertFilter.Apply(imageHistogram);
            ////int t = bestBinaryThFilter.ThresholdValue;
            ////MessageBox.Show("" + t);
            //pictureBox2.Image = imageInvert;
            //pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;

            ////imageSobel = sobelFilter.Apply(imageBinary);
            ////pictureBox2.Image = imageSobel;
            ////pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;


            //////IMAGE EROSION
            ////imageErosion = erosionFilter.Apply(imageInvert);
            ////pictureBox2.Image = imageErosion;
            ////pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;

            ////IMAGE BINARY
            //imageBinary = binaryFilter.Apply(imageInvert);
            //pictureBox2.Image = imageBinary;
            //pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;

            ////IMAGE BINARY 3x3 EROSION
            //imageBinaryErosion = binaryErosionFilter.Apply(imageBinary);
            //pictureBox2.Image = imageBinaryErosion;
            //pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;

            ////imageAccept = Biggestfilter.Apply(imageBinary);
            ////pictureBox2.Image = imageAccept;
            ////pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            #endregion
            
           
            btn_findPlant.Enabled = true;
            
        }

       
        private void ozellikCikart( ) 
        {

            AForge.Imaging.ImageStatistics ims = new AForge.Imaging.ImageStatistics( _imageInvert );
            AForge.Imaging.Blob[] blobs = blobCounter.GetObjectsInformation(); 


            double[] blobAdjustedSize = new double[blobs.Length];
            for ( int i = 0, n = blobs.Length; i < n; i++ )
            {
                edgePoints = blobCounter.GetBlobsEdgePoints( blobs[i] );
                corners = PointsCloud.FindQuadrilateralCorners( edgePoints );
                _fullness = blobs[i].Fullness;
            }

            
            


            //double enBuyukUzunluk = GetGreatestLength( blobs ); //Bunu Zaten Ben Hesaplayabiliyorum
            
            //Graphics grafik = Graphics.FromImage( _imageInvert ); // Burada runtime error var
            //Pen myPen = new Pen( System.Drawing.Color.Red, 15 );
            //DrawEdge( grafik, myPen, edgePoints );
            
           
            
            //_cevre = edgePoints.Count(); // BU BİLGİ KULLANILMAKTA
            double beyazPikselSayisi = ims.PixelsCountWithoutBlack;
            double toplamAlan = ims.PixelsCount;

            //_alanOran = beyazPikselSayisi / toplamAlan;
            //_alanOran = Math.Round( _alanOran, 3 ); // BU BİLGİ KULLANILMAKTA

            double yaprakEni =
                Math.Sqrt(
                Math.Pow( ( corners[0].X - corners[2].X ), 2 ) +
                Math.Pow( ( corners[0].Y - corners[2].Y ), 2 ) );

            double yaprakBoyu = 0.0;
            if ( corners.Count == 3 )
            {
                yaprakBoyu =
                Math.Sqrt(
                Math.Pow( ( corners[0].X - corners[2].X ), 2 ) +
                Math.Pow( ( corners[0].Y - corners[2].Y ), 2 ) );
            }
            else
            {
                yaprakBoyu =
                Math.Sqrt(
                Math.Pow( ( corners[1].X - corners[3].X ), 2 ) +
                Math.Pow( ( corners[1].Y - corners[3].Y ), 2 ) );
            }
            

            _yaprakEni = Math.Round( yaprakEni, 3 ); // BU BİLGİ KULLANILMAKTA
            _yaprakBoyu = Math.Round( yaprakBoyu, 3 ); // BU BİLGİ KULLANILMAKTA
            _yaprakAlan = beyazPikselSayisi;
            _yaprakEnBoyOran = _yaprakEni / yaprakBoyu;
            _yaprakEnBoyOran = Math.Round( _yaprakEnBoyOran, 4 );
            _fullness = Math.Round( _fullness, 3 );

            
            textBox8.Text = "Yaprak Alanı: " + beyazPikselSayisi + " Birim";
            textBox9.Text = "Yaprak Eni: " + _yaprakEni + " Piksel";
            textBox10.Text = "Yaprak Boyu: " + _yaprakBoyu + " Piksel";
            textBox17.Text = "Yaprak En Boy Oranı: " + _yaprakEnBoyOran + " Birim";
            textBox18.Text = "Fullness: " + _fullness + " Birim";
            //textBox11.Text = "Yaprak Çevresi: " + _yaprakCevre + " Birim";
            //textBox16.Text = "Yaprak Yoğunluğu: " + _yaprakAlanCevreOran + " Birim*";
            textBox12.Text = corners[0].ToString();
            textBox13.Text = corners[2].ToString();
            textBox14.Text = corners[1].ToString();
            if ( corners.Count == 3 )
            {
                textBox15.Text = corners[2].ToString();
            }
            else
            textBox15.Text = corners[3].ToString();
        }

        
        private void cevreCiz( )
        {

            Bitmap bmp = new Bitmap( _imageInvert );
            
            for ( int sutun = 1; sutun < _imageInvert.Width - 1; sutun++ )
            {
                int satir = 1;
                
                for (; satir < _imageInvert.Height - 1; satir++ )
                {
                    
                    if ( "ffffffff" == _imageInvert.GetPixel( sutun, satir ).Name.ToLower() )
                    {
                        
                        if (
                            "ffffffff" == _imageInvert.GetPixel( sutun - 1, satir - 1 ).Name.ToLower() &&
                            "ffffffff" == _imageInvert.GetPixel( sutun - 1, satir + 0 ).Name.ToLower() &&
                            "ffffffff" == _imageInvert.GetPixel( sutun - 1, satir + 1 ).Name.ToLower() &&
                            "ffffffff" == _imageInvert.GetPixel( sutun + 0, satir + 1 ).Name.ToLower() &&
                            "ffffffff" == _imageInvert.GetPixel( sutun + 1, satir + 1 ).Name.ToLower() &&
                            "ffffffff" == _imageInvert.GetPixel( sutun + 1, satir + 0 ).Name.ToLower() &&
                            "ffffffff" == _imageInvert.GetPixel( sutun + 1, satir - 1 ).Name.ToLower() &&
                            "ffffffff" == _imageInvert.GetPixel( sutun + 0, satir - 1 ).Name.ToLower()
                            ) 
                        {
                            
                            bmp.SetPixel( sutun, satir, Color.Black );
                           // pictureBox2.Image = bmp;

                           

                        }
                    }
                }
            }


            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            AForge.Imaging.ImageStatistics ims = new AForge.Imaging.ImageStatistics( bmp );
            _yaprakCevre = ims.PixelsCountWithoutBlack; // BU BİLGİ KULLANILMAKTA  
            
                             
            
           for ( int i = 0; i < corners.Count; i++ )
           {
               // MessageBox.Show(corners[i].X + " " + corners[i].Y );
               if ( corners[i].X == bmp.Width - 1 || corners[i].Y == bmp.Height - 1 )
               {
                   bmp.SetPixel( corners[i].X, corners[i].Y, Color.Red );
                   bmp.SetPixel( corners[i].X - 1, corners[i].Y + 0, Color.Red );
                   bmp.SetPixel( corners[i].X - 1, corners[i].Y - 1, Color.Red );
                   bmp.SetPixel( corners[i].X - 1, corners[i].Y - 1, Color.Red );
                   bmp.SetPixel( corners[i].X - 1, corners[i].Y + 0, Color.Red );
                   bmp.SetPixel( corners[i].X - 1, corners[i].Y - 1, Color.Red );
                   bmp.SetPixel( corners[i].X - 1, corners[i].Y - 1, Color.Red );
                   bmp.SetPixel( corners[i].X - 0, corners[i].Y - 1, Color.Red );
                   bmp.SetPixel( corners[i].X - 0, corners[i].Y - 1, Color.Red );
               }
               else if ( corners[i].X == 1 || corners[i].Y == 1 )
               {
                   bmp.SetPixel( corners[i].X, corners[i].Y, Color.Red );
                   bmp.SetPixel( corners[i].X - 0, corners[i].Y + 0, Color.Red );
                   bmp.SetPixel( corners[i].X - 0, corners[i].Y - 0, Color.Red );
                   bmp.SetPixel( corners[i].X - 0, corners[i].Y + 1, Color.Red );
                   bmp.SetPixel( corners[i].X + 1, corners[i].Y + 0, Color.Red );
                   bmp.SetPixel( corners[i].X + 1, corners[i].Y - 0, Color.Red );
                   bmp.SetPixel( corners[i].X + 1, corners[i].Y + 1, Color.Red );
                   bmp.SetPixel( corners[i].X - 0, corners[i].Y - 0, Color.Red );
                   bmp.SetPixel( corners[i].X - 0, corners[i].Y + 1, Color.Red );
               }
               else if ( corners[i].X == 0 || corners[i].Y == 0 )
               {
                   bmp.SetPixel( corners[i].X, corners[i].Y, Color.Red );
                   bmp.SetPixel( corners[i].X + 1, corners[i].Y + 0, Color.Red );
                   bmp.SetPixel( corners[i].X + 1, corners[i].Y + 1, Color.Red );
                   bmp.SetPixel( corners[i].X + 1, corners[i].Y + 1, Color.Red );
                   bmp.SetPixel( corners[i].X + 1, corners[i].Y + 0, Color.Red );
                   bmp.SetPixel( corners[i].X + 1, corners[i].Y + 1, Color.Red );
                   bmp.SetPixel( corners[i].X + 1, corners[i].Y + 1, Color.Red );
                   bmp.SetPixel( corners[i].X - 0, corners[i].Y + 1, Color.Red );
                   bmp.SetPixel( corners[i].X - 0, corners[i].Y + 1, Color.Red );
               }
               else
               {
                   bmp.SetPixel( corners[i].X, corners[i].Y, Color.Red );
                   bmp.SetPixel( corners[i].X + 1, corners[i].Y + 0, Color.Red );
                   bmp.SetPixel( corners[i].X + 1, corners[i].Y + 1, Color.Red );
                   bmp.SetPixel( corners[i].X + 1, corners[i].Y - 1, Color.Red );
                   bmp.SetPixel( corners[i].X - 1, corners[i].Y + 0, Color.Red );
                   bmp.SetPixel( corners[i].X - 1, corners[i].Y + 1, Color.Red );
                   bmp.SetPixel( corners[i].X - 1, corners[i].Y - 1, Color.Red );
                   bmp.SetPixel( corners[i].X - 0, corners[i].Y + 1, Color.Red );
                   bmp.SetPixel( corners[i].X - 0, corners[i].Y - 1, Color.Red );
               }
               
           }
           pictureBox2.Image = bmp;

           textBox11.Text = "Yaprak Çevresi: " + _yaprakCevre + " Birim";
           _yaprakAlanCevreOran = ( 4 * Math.PI * _yaprakAlan ) / Math.Pow( _yaprakCevre, 2 ); // (4pi * area)
           
                                                                           // compactness =  ______________________
                                                                           //                (perimeter)*(perimeter)

           _yaprakAlanCevreOran = Math.Round( _yaprakAlanCevreOran, 4 );
           
           textBox16.Text = "Yaprak Yoğunluğu: " + _yaprakAlanCevreOran + " Birim*";
           trackBar1.Value = 250;

           
            
        }


        private void binaryAl( )
        {
            // Burada yaratılan filtrelerin hepsini kullanmıyoruz. Sadece gerekli olanları kullanıyoruz.
            Grayscale grayFilter = new Grayscale( 0.2125, 0.7154, 0.0721 );
            Threshold binaryFilter = new Threshold( 150 );
            IterativeThreshold iteratifThFilter = new IterativeThreshold( trackBar1.Value, 128 );
            SobelEdgeDetector sobelFilter = new SobelEdgeDetector();
            OtsuThreshold bestBinaryThFilter = new OtsuThreshold();
            ContrastStretch contrastFilter = new ContrastStretch();
            Invert invertFilter = new Invert();
            Erosion erosionFilter = new Erosion();
            BinaryErosion3x3 binaryErosionFilter = new BinaryErosion3x3();
            Dilatation dilationFilter = new Dilatation();
            Opening openinFilter = new Opening();
            Closing closingFilter = new Closing();
            ExtractBiggestBlob biggestFilter = new ExtractBiggestBlob();
            HistogramEqualization histogramEqualizationFilter = new HistogramEqualization();
            BilateralSmoothing smoothingFilter = new BilateralSmoothing();



            Bitmap bmp = new Bitmap( image );


            _imageGrayScale = grayFilter.Apply( bmp );
            pictureBox2.Image = _imageGrayScale;
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            
            

            _imageContrast = contrastFilter.Apply( _imageGrayScale );
            pictureBox2.Image = _imageContrast;
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            

            smoothingFilter.KernelSize = 15;
            smoothingFilter.SpatialFactor = 250;
            smoothingFilter.ColorFactor = trackBar1.Value;
            label1.Text = "Bozulma Oranı Yaklaşık %" + (trackBar1.Value / 100.0).ToString();
            smoothingFilter.ColorPower = 1.5;
            _imageContrast = smoothingFilter.Apply( _imageContrast );
            pictureBox2.Image = _imageContrast;
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
           


            _imageOtsu = bestBinaryThFilter.Apply( _imageContrast );
            pictureBox2.Image = _imageOtsu;
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            
            
            

            _imageInvert = invertFilter.Apply( _imageOtsu );
            pictureBox2.Image = _imageInvert;
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            


            if ( objeSayisi != 1 )
            {
                objeSay();
            }

            

            
        }


        private void objeSay( )
        {


            blobCounter = new AForge.Imaging.BlobCounter( _imageInvert );

            objeSayisi = blobCounter.ObjectsCount;
            
            //MessageBox.Show( objeSayisi + "" );
            if ( trackBar1.Value == 1000 )
            {
                trackBar1.Value = 1000;
            }
            else
            trackBar1.Value++;

            binaryAl();
        }


        private void enBuyukParcaAl( )
        {
            ExtractBiggestBlob filter = new ExtractBiggestBlob();
            // apply the filter
            Bitmap biggestBlobsImage = filter.Apply( _imageInvert );

            MessageBox.Show( biggestBlobsImage.Width + "x" + biggestBlobsImage.Height );
        }


        private void trackBar2_Scroll( object sender, EventArgs e )
        {
            label3.Text = "Gizli Katman: " + trackBar2.Value.ToString();
        }

        private void trackBar3_Scroll( object sender, EventArgs e )
        {
            label2.Text = "İterasyon: " + trackBar3.Value.ToString();
        }

        private void trackBar4_Scroll( object sender, EventArgs e )
        {
            label4.Text = "Learning Rate: " + (trackBar4.Value / 1000.0).ToString(); 
        }
        
        // VISIBLE FALSE
        private void button2_Click_1( object sender, EventArgs e )
        {
            try
            {
                StreamWriter yaz;

                yaz = File.AppendText( "C:\\Users\\OSMAN\\documents\\visual studio 2013\\Projects\\WindowsFormsApplication2\\WindowsFormsApplication2\\data.txt" );
                yaz.WriteLine( "asdasdsadsa" + ";" + _yaprakAlanCevreOran  + ";" + _yaprakEni + ";" + _yaprakBoyu + ";" + _yaprakCevre );

                yaz.Close();

                MessageBox.Show( "Kayıt Başarılı" );
            }
            catch ( Exception )
            {

                MessageBox.Show( "Kayıt Yapılamadı" );

            }

        }
        /////////////////
        private void btn_findPlant_Click( object sender, EventArgs e )
        {
            double alanCevreOran = _yaprakAlanCevreOran;
            double en = _yaprakEni;
            double boy = _yaprakBoyu;
            double enBoyOran = _yaprakEnBoyOran;
            double fullness = _fullness;
            double cevreKenar = _yaprakCevreKenarOran;


            alanCevreOran = _yaprakAlanCevreOran;
            //en = en / 1000.0;
            //boy = boy / 1000.0;


            double[] girdi = { alanCevreOran, enBoyOran, fullness, cevreKenar };

            Network network = Network.Load( "C:\\Users\\OSMAN\\documents\\visual studio 2013\\Projects\\WindowsFormsApplication2\\WindowsFormsApplication2\\my_nn.bin" );
            
            textBox1.Text ="Max. Output: " + network.Compute( girdi ).Max().ToString();
            
            
            double maxResult;
            double[] result = new double[10];

            

            //for ( int i = 0; i < network.Output.Length; i++ )
            //{
            //    if ( network.Output[i] == 1.0 || network.Output[i] == -1.0 )
            //    {
            //        result[i] = 0;
            //    }
            //    else
            //    {
            //        result[i] = Math.Abs( network.Output[i] );
            //    }
            //}

                //MessageBox.Show( dlg.FileName + "\n" +
                //    result[0] + "\n" +
                //    result[1] + "\n" +
                //    result[2] + "\n" +
                //    result[3] + "\n" +
                //    result[4] + "\n" +
                //    result[5] + "\n" +
                //    result[6] + "\n" +
                //    result[7] + "\n" +
                //    result[8] + "\n" +
                //    result[9]
                //    );

            MessageBox.Show( dlg.FileName + "\n" +
                    network.Output[0] + "\n" +
                    network.Output[1] + "\n" +
                    network.Output[2] + "\n" +
                    network.Output[3] + "\n" +
                    network.Output[4] + "\n" +
                    network.Output[5] + "\n" +
                    network.Output[6] + "\n" +
                    network.Output[7] + "\n" +
                    network.Output[8] + "\n" +
                    network.Output[9]
                    );
                maxResult = result.Max();
            
            for ( int i = 0; i < network.Output.Length; i++ )
            {
                if ( Math.Abs(network.Output[i]) == maxResult )
                {
                    int outputPlantLeaf = i + 1;
                    textBox2.Text = "Bulunan Tür: " + outputPlantLeaf.ToString();
                    break;
            //        // Dosya isminden tür bilgisi alma
            //        string sonuc = "";
            //        string secilenTur;
            //        string secilenTurYolu = dlg.FileName;
            //        int yolUzunlugu = secilenTurYolu.Length;
            //        secilenTur = secilenTurYolu.Remove( 0, 28 );
            //        secilenTur = secilenTur.Replace( ".jpg", "" );

            //        if ( secilenTur == outputPlantLeaf.ToString() )
            //        {
            //            sonuc = "Doğru";
            //        }
            //        else
            //            sonuc = "Yanlış";

            //        MessageBox.Show( "Seçilen Tür: " + secilenTur + "\n" + "Bulunan Tür: " + outputPlantLeaf + "\n" + "Sonuç: " + sonuc );
               }
            }

            

            

            //if ( find > -1.0 && find < 1.0 )
              //  MessageBox.Show( "1-Acer monspessulanum" );
            //if(find >= 0.5 && find <= 0.7 )
                // MessageBox.Show ( "2" );


            

        }

        private void btn_deneme_Click( object sender, EventArgs e )
        {
            string filePath = "";
            //ImageList imList = new ImageList();
            List<Image> imageList = new List<Image>();

            for ( dosyaSayisi = 1; dosyaSayisi < 11; dosyaSayisi++ )
            {

                filePath = @"C:\Users\OSMAN\documents\visual studio 2013\Projects\WindowsFormsApplication2\WindowsFormsApplication2\Yaprak2\" + dosyaSayisi;
                string[] filePaths = Directory.GetFiles( filePath, "*.jpg" );
                
                imageList.Clear();
                //StreamWriter yaz1;
                //yaz1 = File.AppendText( "C:\\Users\\OSMAN\\documents\\visual studio 2013\\Projects\\WindowsFormsApplication2\\WindowsFormsApplication2\\dosyaSayisi.txt" );
                //yaz1.WriteLine( filePaths.Length + "" );
                //yaz1.Close();

                for ( int i = 0; i < filePaths.Length; i++ )
                {

                    imageList.Add( Image.FromFile( filePaths[i] ) );
                  //  imList.Images.Add( Image.FromFile( filePaths[i] ) );
                    
                }

                for ( int i = 0; i < imageList.Count; i++ )
                {
                    image = imageList[i];

                    binaryAl();

                    ozellikCikart();

                    cevreCiz();

                    _yaprakCevreKenarOran = ( _yaprakBoyu + _yaprakEni ) / _yaprakCevre;
                    _yaprakCevreKenarOran = Math.Round( _yaprakCevreKenarOran, 3 );

                    Application.DoEvents();

                   

                    try
                    {
                        StreamWriter yaz;

                        yaz = File.AppendText( "C:\\Users\\OSMAN\\documents\\visual studio 2013\\Projects\\WindowsFormsApplication2\\WindowsFormsApplication2\\deneme1234.txt" );
                        yaz.WriteLine( dosyaSayisi + ";" + _yaprakAlanCevreOran + ";" + _yaprakEnBoyOran + ";" + _fullness + ";" + _yaprakCevreKenarOran );

                        yaz.Close();
                        //yaz = File.AppendText( "C:\\Users\\OSMAN\\documents\\visual studio 2013\\Projects\\WindowsFormsApplication2\\WindowsFormsApplication2\\dataAspectRatio.txt" );
                        //yaz.WriteLine( _yaprakEnBoyOran );

                        //yaz.Close();

                        //yaz1 = File.AppendText( "C:\\Users\\OSMAN\\documents\\visual studio 2013\\Projects\\WindowsFormsApplication2\\WindowsFormsApplication2\\dataCompactness.txt" );
                        //yaz1.WriteLine( _yaprakAlanCevreOran );

                        //yaz1.Close();
                        
                        //yaz2 = File.AppendText( "C:\\Users\\OSMAN\\documents\\visual studio 2013\\Projects\\WindowsFormsApplication2\\WindowsFormsApplication2\\dataFullness.txt" );
                        //yaz2.WriteLine( _fullness );

                        //yaz2.Close();
                        

                        
                        //string input = "";

                        //if ( dosyaSayisi == 1 )
                        //{
                        //    input = "1.0;-1.0;-1.0;-1.0;-1.0;-1.0;-1.0;-1.0;-1.0;-1.0";
                        //}
                        //if ( dosyaSayisi == 2 )
                        //{
                        //    input = "-1.0;1.0;-1.0;-1.0;-1.0;-1.0;-1.0;-1.0;-1.0;-1.0";
                        //}
                        //if ( dosyaSayisi == 3 )
                        //{
                        //    input = "-1.0;-1.0;1.0;-1.0;-1.0;-1.0;-1.0;-1.0;-1.0;-1.0";
                        //}
                        //if ( dosyaSayisi == 4 )
                        //{
                        //    input = "-1.0;-1.0;-1.0;1.0;-1.0;-1.0;-1.0;-1.0;-1.0;-1.0";
                        //}
                        //if ( dosyaSayisi == 5 )
                        //{
                        //    input = "-1.0;-1.0;-1.0;-1.0;1.0;-1.0;-1.0;-1.0;-1.0;-1.0";
                        //}
                        //if ( dosyaSayisi == 6 )
                        //{
                        //    input = "-1.0;-1.0;-1.0;-1.0;-1.0;1.0;-1.0;-1.0;-1.0;-1.0";
                        //}
                        //if ( dosyaSayisi == 7 )
                        //{
                        //    input = "-1.0;-1.0;-1.0;-1.0;-1.0;-1.0;1.0;-1.0;-1.0;-1.0";
                        //}
                        //if ( dosyaSayisi == 8 )
                        //{
                        //    input = "-1.0;-1.0;-1.0;-1.0;-1.0;-1.0;-1.0;1.0;-1.0;-1.0";
                        //}
                        //if ( dosyaSayisi == 9 )
                        //{
                        //    input = "-1.0;-1.0;-1.0;-1.0;-1.0;-1.0;-1.0;-1.0;1.0;-1.0";
                        //}
                        //if ( dosyaSayisi == 10 )
                        //{
                        //    input = "-1.0;-1.0;-1.0;-1.0;-1.0;-1.0;-1.0;-1.0;-1.0;1.0";
                        //}

                        //    sadeceInput = File.AppendText( "C:\\Users\\OSMAN\\documents\\visual studio 2013\\Projects\\WindowsFormsApplication2\\WindowsFormsApplication2\\data3.txt" );
                        //sadeceInput.WriteLine( _yaprakAlanCevreOran + ";" + _yaprakEnBoyOran + ";" + _fullness + ";" + input );
                        //sadeceInput.Close();
                        objeSayisi = 100;

                        //MessageBox.Show( "Kayıt Başarılı" );
                    }
                    catch ( Exception )
                    {

                        MessageBox.Show( "Kayıt Yapılamadı" );

                    }
                    //resetValues();


                }




            }   
        }


        private void trackBar1_Scroll( object sender, EventArgs e )
        {
            label1.Text = "Bozulma Oranı Yaklaşık %" + trackBar1.Value / 100.0;
        }

        //public void resetValues( )
        //{
        //    textBox1.Text = "";
        //    textBox2.Text = "";
        //    textBox3.Text = "";
        //    textBox4.Text = "";
        //    textBox5.Text = "";
        //    textBox6.Text = "";
        //    textBox7.Text = "";
        //    textBox8.Text = "";
        //    textBox9.Text = "";
        //    textBox10.Text = "";
        //    textBox11.Text = "";
        //    textBox12.Text = "";
        //    textBox13.Text = "";
        //    textBox14.Text = "";
        //    textBox15.Text = "";
        //    textBox16.Text = "";
        //    label1.Text = "";
        //    label2.Text = "";
        //    label3.Text = "";
        //    label4.Text = "";
        //    label5.Text = "";
        //    label6.Text = "";
        //    label7.Text = "";
        //    label8.Text = "";
        //    network = null;
        //    dongu = false;
        //    _yaprakCevre = 0.0;
        //    _yaprakEni = 0.0;
        //    _yaprakBoyu = 0.0;
        //    _yaprakAlanCevreOran = 0.0;
        //    _yaprakAlan = 0.0;
        //    image = null;
        //    dlg = null;
        //    blobCounter = null;
        //    edgePoints = null;
        //    corners = null;
        //    objeSayisi = 100;
        //    _imageGrayScale = null;
        //    _imageHistogram = null;
        //    _imageBinary = null;
        //    _imageIteratif = null;
        //    _imageSobel = null;
        //    _imageOtsu = null;
        //    _imageContrast = null;
        //    _imageInvert = null;
        //    _imageErosion = null;
        //    _imageBinaryErosion = null;
        //    _imageDilation = null;
        //    _imageOpening = null;
        //    _imageClosing = null;
        //    _imageBiggest = null;
        //    _imageAccept = null;
        //    _inputData = null;
        //    _outputData = null;
            
        //}

        //BURASINI DAHA KISA OLACAK ŞEKİLDE YUKARIDA YAZDIM VE ASLINDA BURASI KULLANILMIYOR
        private static System.Drawing.Point[] PointsListToArray( List<IntPoint> list )
        {
            System.Drawing.Point[] array = new System.Drawing.Point[list.Count];

            for ( int i = 0, n = list.Count; i < n; i++ )
            {
                array[i] = new System.Drawing.Point( list[i].X, list[i].Y );
            }

            return array;
        }

        // Draw object's edge
        private static void DrawEdge( Graphics g, Pen pen, List<IntPoint> edge )
        {
            System.Drawing.Point[] points = PointsListToArray( edge );

            if ( points.Length > 1 )
            {
                g.DrawLines( pen, points );
            }
            else
            {
                g.DrawLine( pen, points[0], points[0] );
            }
        }


        /// <summary>
        /// Burada yaprağın en uzun iki kenarı arası mesafe hesaplanıyor.
        /// </summary>
        /// <param name="blob"></param>
        /// <returns></returns>

        private double GetGreatestLength( AForge.Imaging.Blob[] blob )
        {
            try
            {
                GrahamConvexHull hullFinder = new GrahamConvexHull();

                var hullPoints = hullFinder.FindHull( edgePoints );
                var maxPoints = GetMaxPoints( hullPoints );

                return maxPoints.distance * 1; // 1 olan yer aslında microns per pixel
            }
            catch ( Exception )
            {
                return 0;
            }
        }

        struct MaxPoints
        {
            public IntPoint firstPoint;
            public IntPoint secondPoint;
            public double distance;

            public MaxPoints( IntPoint first, IntPoint second, double dist )
            {
                this.firstPoint = first;
                this.secondPoint = second;
                this.distance = dist;
            }
        }

        private MaxPoints GetMaxPoints( IEnumerable<IntPoint> points )
        {
            var data = from a in points
                       from b in points
                       select new MaxPoints( a, b, GetDistance( a, b ) );

            double maxDistance = data.Max( d => d.distance );

            return data.First( d => d.distance == maxDistance );
        }

        private double GetDistance( IntPoint a, IntPoint b )
        {
            return Math.Sqrt( Math.Pow( ( a.X - b.X ), 2 ) + Math.Pow( ( a.Y - b.Y ), 2 ) );
        }

        private void openFileDialog1_FileOk( object sender, CancelEventArgs e )
        {

        }

        private void openFileDialog_FileOk( object sender, CancelEventArgs e )
        {

        }

        private void timer1_Tick( object sender, EventArgs e )
        {
             
            
        }

        private void btn_ABC_Click( object sender, EventArgs e )
        {
            List<double[]> inputs = new List<double[]>();
            List<double[]> outputs = new List<double[]>();

            string testData = "C: \\Users\\OSMAN\\documents\\visual studio 2013\\Projects\\WindowsFormsApplication2\\WindowsFormsApplication2\\D.txt";
            string saveData = "C:\\Users\\OSMAN\\documents\\visual studio 2013\\Projects\\WindowsFormsApplication2\\WindowsFormsApplication2\\nn_ABC.bin";
            #region DataOkuma
            using ( StreamReader okuyucu = File.OpenText( _DATAM1 ) )
            {

                okuyucu.ReadLine();

                while ( !okuyucu.EndOfStream )
                {
                    string satir = okuyucu.ReadLine();
                    string[] bolum = satir.Split( ';' );

                    double[] input = new double[INPUT_SAYISI];
                    double[] output = new double[OUTPUT_SAYISI];

                    //Get data
                    for ( int i = 0; i < INPUT_SAYISI; i++ )
                    {

                        if ( i == 1 )
                        {
                            input[i] = double.Parse( bolum[i + 1], NumberFormatInfo.InvariantInfo );
                        }
                        else
                            input[i] = double.Parse( bolum[i + 1], NumberFormatInfo.InvariantInfo );

                        if ( i == 0 )
                        {
                            #region OUTPUTS DECLARETING
                            if ( bolum[i].ToString() == "1" )
                            {
                                output[i + 0] = 1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                                
                            }
                            else if ( bolum[i].ToString() == "2" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = 1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "3" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = 1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "4" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = 1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "5" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = 1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "6" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = 1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                                
                            }
                            else if ( bolum[i].ToString() == "7" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = 1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "8" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = 1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "9" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = 1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "10" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = 1.0;
                            }
                            
                            #endregion
                        }
                    }
                    outputs.Add( output );
                    inputs.Add( input );
                }

            }
            #endregion
            _inputData = inputs.ToArray();
            _outputData = outputs.ToArray();
            
            normalizeEt1( _inputData ); // normalize first inputData (compactness) [0,1]
            normalizeEt2( _inputData ); // normalize second inputData (enBoyOran) [0,1]
            normalizeEt3( _inputData ); // normalize third inputData (fullness) [0,1]

            //daha sonra try-catch koyulacak
            MessageBox.Show( "ABC Data Loaded" );
            btn_trainIris.Enabled = true;

            trainData( _inputData, _outputData );

            network.Save( saveData );
            MessageBox.Show( "Trained Successful" );
            btn_yaprakSec.Enabled = true;

            readTestData(testData);

            findResult(_inputData,_outputData,saveData);




        }

        private void btn_ABD_Click( object sender, EventArgs e )
        {
            List<double[]> inputs = new List<double[]>();
            List<double[]> outputs = new List<double[]>();

            string testData = "C:\\Users\\OSMAN\\documents\\visual studio 2013\\Projects\\WindowsFormsApplication2\\WindowsFormsApplication2\\C.txt";
            string saveData = "C:\\Users\\OSMAN\\documents\\visual studio 2013\\Projects\\WindowsFormsApplication2\\WindowsFormsApplication2\\nn_ABD.bin";

            #region DataOkuma
            using ( StreamReader okuyucu = File.OpenText( _DATAM2 ) )
            {

                okuyucu.ReadLine();

                while ( !okuyucu.EndOfStream )
                {
                    string satir = okuyucu.ReadLine();
                    string[] bolum = satir.Split( ';' );

                    double[] input = new double[INPUT_SAYISI];
                    double[] output = new double[OUTPUT_SAYISI];

                    //Get data
                    for ( int i = 0; i < INPUT_SAYISI; i++ )
                    {

                        if ( i == 1 )
                        {
                            input[i] = double.Parse( bolum[i + 1], NumberFormatInfo.InvariantInfo );
                        }
                        else
                            input[i] = double.Parse( bolum[i + 1], NumberFormatInfo.InvariantInfo );

                        if ( i == 0 )
                        {
                            #region OUTPUTS DECLARETING
                            if ( bolum[i].ToString() == "1" )
                            {
                                output[i + 0] = 1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "2" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = 1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "3" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = 1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "4" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = 1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "5" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = 1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "6" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = 1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "7" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = 1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "8" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = 1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "9" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = 1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "10" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = 1.0;
                            }
                            
                            #endregion
                        }
                    }
                    outputs.Add( output );
                    inputs.Add( input );
                }

            }
            #endregion
            _inputData = inputs.ToArray();
            _outputData = outputs.ToArray();

            normalizeEt1( _inputData ); // normalize first inputData (compactness) [0,1]
            normalizeEt2( _inputData ); // normalize second inputData (enBoyOran) [0,1]
            normalizeEt3( _inputData ); // normalize third inputData (fullness) [0,1]

            //daha sonra try-catch koyulacak
            MessageBox.Show( "ABD Data Loaded" );
            btn_trainIris.Enabled = true;

            trainData( _inputData, _outputData );

            network.Save( saveData );
            MessageBox.Show( "Trained Successful" );
            btn_yaprakSec.Enabled = true;

            readTestData( testData );

            findResult( _inputData, _outputData, saveData );

        }

        private void btn_BCD_Click( object sender, EventArgs e )
        {
            List<double[]> inputs = new List<double[]>();
            List<double[]> outputs = new List<double[]>();

            string testData = "C: \\Users\\OSMAN\\documents\\visual studio 2013\\Projects\\WindowsFormsApplication2\\WindowsFormsApplication2\\A.txt";
            string saveData = "C:\\Users\\OSMAN\\documents\\visual studio 2013\\Projects\\WindowsFormsApplication2\\WindowsFormsApplication2\\nn_BCD.bin";

            #region DataOkuma
            using ( StreamReader okuyucu = File.OpenText( _DATAM3 ) )
            {

                okuyucu.ReadLine();

                while ( !okuyucu.EndOfStream )
                {
                    string satir = okuyucu.ReadLine();
                    string[] bolum = satir.Split( ';' );

                    double[] input = new double[INPUT_SAYISI];
                    double[] output = new double[OUTPUT_SAYISI];

                    //Get data
                    for ( int i = 0; i < INPUT_SAYISI; i++ )
                    {

                        if ( i == 1 )
                        {
                            input[i] = double.Parse( bolum[i + 1], NumberFormatInfo.InvariantInfo );
                        }
                        else
                            input[i] = double.Parse( bolum[i + 1], NumberFormatInfo.InvariantInfo );

                        if ( i == 0 )
                        {
                            #region OUTPUTS DECLARETING
                            if ( bolum[i].ToString() == "1" )
                            {
                                output[i + 0] = 1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "2" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = 1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "3" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = 1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "4" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = 1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "5" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = 1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "6" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = 1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "7" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = 1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "8" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = 1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "9" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = 1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "10" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = 1.0;
                            }
                            
                            #endregion
                        }
                    }
                    outputs.Add( output );
                    inputs.Add( input );
                }

            }
            #endregion
            _inputData = inputs.ToArray();
            _outputData = outputs.ToArray();

            normalizeEt1( _inputData ); // normalize first inputData (compactness) [0,1]
            normalizeEt2( _inputData ); // normalize second inputData (enBoyOran) [0,1]
            normalizeEt3( _inputData ); // normalize third inputData (fullness) [0,1]

            //daha sonra try-catch koyulacak
            MessageBox.Show( "BCD Data Loaded" );
            btn_trainIris.Enabled = true;

            trainData( _inputData, _outputData );

            network.Save( saveData );
            MessageBox.Show( "Trained Successful" );
            btn_yaprakSec.Enabled = true;

            readTestData( testData );

            findResult( _inputData, _outputData, saveData );

        }

        private void btn_ACD_Click( object sender, EventArgs e )
        {
            List<double[]> inputs = new List<double[]>();
            List<double[]> outputs = new List<double[]>();

            string testData = "C: \\Users\\OSMAN\\documents\\visual studio 2013\\Projects\\WindowsFormsApplication2\\WindowsFormsApplication2\\B.txt";
            string saveData = "C:\\Users\\OSMAN\\documents\\visual studio 2013\\Projects\\WindowsFormsApplication2\\WindowsFormsApplication2\\nn_ACD.bin";

            #region DataOkuma
            using ( StreamReader okuyucu = File.OpenText( _DATAM4 ) )
            {

                okuyucu.ReadLine();

                while ( !okuyucu.EndOfStream )
                {
                    string satir = okuyucu.ReadLine();
                    string[] bolum = satir.Split( ';' );

                    double[] input = new double[INPUT_SAYISI];
                    double[] output = new double[OUTPUT_SAYISI];

                    //Get data
                    for ( int i = 0; i < INPUT_SAYISI; i++ )
                    {

                        if ( i == 1 )
                        {
                            input[i] = double.Parse( bolum[i + 1], NumberFormatInfo.InvariantInfo );
                        }
                        else
                            input[i] = double.Parse( bolum[i + 1], NumberFormatInfo.InvariantInfo );

                        if ( i == 0 )
                        {
                            #region OUTPUTS DECLARETING
                            if ( bolum[i].ToString() == "1" )
                            {
                                output[i + 0] = 1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "2" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = 1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "3" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = 1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "4" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = 1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "5" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = 1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "6" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = 1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "7" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = 1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "8" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = 1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "9" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = 1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "10" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = 1.0;
                            }
                            
                            #endregion
                        }
                    }
                    outputs.Add( output );
                    inputs.Add( input );
                }

            }
            #endregion
            _inputData = inputs.ToArray();
            _outputData = outputs.ToArray();

            normalizeEt1( _inputData ); // normalize first inputData (compactness) [0,1]
            normalizeEt2( _inputData ); // normalize second inputData (enBoyOran) [0,1]
            normalizeEt3( _inputData ); // normalize third inputData (fullness) [0,1]   

            //daha sonra try-catch koyulacak
            MessageBox.Show( "ACD Data Loaded" );
            btn_trainIris.Enabled = true;

            trainData( _inputData, _outputData );

            network.Save( saveData );
            MessageBox.Show( "Trained Successful" );
            btn_yaprakSec.Enabled = true;

            readTestData( testData );

            findResult( _inputData, _outputData, saveData );
        }

        private void findResult( double[][] _inputData, double[][] _outputData, string saveData )
        {
            Network network =
                Network.Load( saveData );
            //double[] girdi;

            for ( int i = 0; i < this._inputData.Length; i++ )
            {
                double[] girdi = { this._inputData[i][0], this._inputData[i][1], this._inputData[i][2] };
                network.Compute( girdi );
                // BURADA BULUNAN BİTKİNİN TÜRÜNÜ ve ASLINDA OLAN BİTKİ TÜRÜNÜ DOSYAYA YAZACAK
                findPlantLeaf( network.Output, _outputData, i );

            }
        }

        private void findPlantLeaf( double[] p, double[][] _outputData, int i )
        {

            double bulunanTur = 0.0;
            double gercekTur = 0.0;

            for ( int j = 0; j < p.Length; j++ )
            {
                if ( p[j] == p.Max() )
                {
                    bulunanTur = j + 1;
                }

                if ( this._outputData[i][j] == 1.0 )
                {
                    gercekTur = j + 1;
                }

                if ( bulunanTur != 0.0 && gercekTur != 0.0 )
                {
                    break;
                }
            }

            
            resultSave( bulunanTur, gercekTur );

        }

        private void resultSave( double bulunanTur, double gercekTur )
        {
            StreamWriter resultWriter;
            string resultSavePath = "C:\\Users\\OSMAN\\documents\\visual studio 2013\\Projects\\WindowsFormsApplication2\\WindowsFormsApplication2\\resultSave.txt";

            bool tespit = false;
            if ( bulunanTur == gercekTur )
            {
                tespit = true;
            }



            resultWriter = File.AppendText( resultSavePath );
            resultWriter.WriteLine( Convert.ToInt32(bulunanTur) + ";" + Convert.ToInt32(gercekTur) + ";" + Convert.ToInt32(tespit) );
            resultWriter.Close();

        }

        private void readTestData( string testData )
        {
            List<double[]> inputs = new List<double[]>();
            List<double[]> outputs = new List<double[]>();


            #region DataOkuma
            using ( StreamReader okuyucu = File.OpenText( testData ) )
            {

                okuyucu.ReadLine();

                while ( !okuyucu.EndOfStream )
                {
                    string satir = okuyucu.ReadLine();
                    string[] bolum = satir.Split( ';' );

                    double[] input = new double[INPUT_SAYISI];
                    double[] output = new double[OUTPUT_SAYISI];

                    //Get data
                    for ( int i = 0; i < INPUT_SAYISI; i++ )
                    {


                        input[i] = double.Parse( bolum[i + 1], NumberFormatInfo.InvariantInfo );

                        if ( i == 0 )
                        {
                            #region OUTPUTS DECLARETING
                            if ( bolum[i].ToString() == "1" )
                            {
                                output[i + 0] = 1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "2" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = 1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "3" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = 1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "4" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = 1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "5" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = 1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "6" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = 1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "7" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = 1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "8" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = 1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "9" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = 1.0;
                                output[i + 9] = -1.0;
                            }
                            else if ( bolum[i].ToString() == "10" )
                            {
                                output[i + 0] = -1.0;
                                output[i + 1] = -1.0;
                                output[i + 2] = -1.0;
                                output[i + 3] = -1.0;
                                output[i + 4] = -1.0;
                                output[i + 5] = -1.0;
                                output[i + 6] = -1.0;
                                output[i + 7] = -1.0;
                                output[i + 8] = -1.0;
                                output[i + 9] = 1.0;
                            }
                            
                            #endregion
                        }
                    }
                    outputs.Add( output );
                    inputs.Add( input );
                }

            }
            #endregion




            _inputData = inputs.ToArray();
            _outputData = outputs.ToArray();




            //daha sonra try-catch koyulacak
            MessageBox.Show( "Plants Data Loaded" );
            btn_trainIris.Enabled = true;
            btn_yaprakSec.Enabled = true;

            normalizeEt1( _inputData ); // normalize first inputData (compactness) [0,1]
            normalizeEt2( _inputData ); // normalize second inputData (enBoyOran) [0,1]
            normalizeEt3( _inputData ); // normalize third inputData (fullness) [0,1]

            //normalizedInputDataWriteToFile( _inputData );
            //MessageBox.Show("Dönüşümler Başarılı");

        }


    }
}
        #endregion
