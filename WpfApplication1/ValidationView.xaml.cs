using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Logique d'interaction pour ValidationView.xaml
    /// </summary>
    public partial class ValidationView : Page
    {
        public ValidationView()
        {
            InitializeComponent();
        }

        private string _course;
        private string _allRanking;
        private int _id;
        private string _mailT;
        private ComboBox _notationValue;
        private ComboBox _listQuest;
        private TextBox _inputEcts;
        private TextBlock _questionValue;
        private TextBox _mail;
        private PasswordBox _password;
        private TextBox _ip;
        private TextBox _port;
        private TextBox _subject;
        private TextBox _body;

        public void selectId(int id)
        {
            Database1Entities1 data = new Database1Entities1();

            if (data.Teacher.Find(id) == null)
            {
                TextBlock infoId = new TextBlock();
                infoId.Text = "Bad ID. enter correct ID !";
                infoId.Foreground = new SolidColorBrush(Colors.Red);
                Grid.SetRow(infoId, 2);
                Grid.SetColumn(infoId, 0);
                dynamiqueGrid.Children.Add(infoId);
            }
            else
                selectCourse(id);

        }

        public void selectCourse(int id)
        {
            dynamiqueGrid.Children.Clear();

            _id = id;

            Database1Entities1 data = new Database1Entities1();

            TextBlock infoCourse = new TextBlock();
            infoCourse.Text = "Courses : ";
            infoCourse.Foreground = new SolidColorBrush(Colors.White);
            Grid.SetRow(infoCourse, 0);
            Grid.SetColumn(infoCourse, 0);

            TextBlock infoCourseWould = new TextBlock();
            _course = data.Courses.Find(data.Teacher.Find(_id).IdCoursesWould).CodeMat;
            infoCourseWould.Text = _course;
            infoCourseWould.Foreground = new SolidColorBrush(Colors.Red);
            Grid.SetRow(infoCourseWould, 1);
            Grid.SetColumn(infoCourseWould, 0);

            Button valideCourse = new Button();
            valideCourse.Content = "Select this course";
            valideCourse.Click += new System.Windows.RoutedEventHandler(valide_course); 
            Grid.SetRow(valideCourse, 2);
            Grid.SetColumn(valideCourse, 0);

            dynamiqueGrid.Children.Add(infoCourse);
            dynamiqueGrid.Children.Add(infoCourseWould);
            dynamiqueGrid.Children.Add(valideCourse);

        }

        private void valide_course(object sender, EventArgs e)
        {
            dynamiqueGrid.Children.Clear();

            Database1Entities1 data = new Database1Entities1();

            if (data.Courses.Find(_course) != null)
            {
                dynamiqueGrid.Children.Clear();
                selectQuestion(_id);
            }   
            else
                selectCourse(_id);

        } 

        public void selectQuestion(int id)
        {
            Database1Entities1 data = new Database1Entities1();

            TextBlock infoQuestion = new TextBlock();
            infoQuestion.Text = "Select the questions :";
            infoQuestion.Width = 150;
            infoQuestion.Foreground = new SolidColorBrush(Colors.White);
            Grid.SetRow(infoQuestion, 0);
            Grid.SetColumn(infoQuestion, 0);

            _listQuest = new ComboBox();
            _listQuest.Width = 75;
            _listQuest.SelectionChanged += ComboBox_SelectionChanged;

            foreach (Questions source in data.Questions.ToArray())
            {
                if (source.CodeMat == _course)
                    _listQuest.Items.Add("ID : " + source.Id + "Question : " + source.Question);
            }
            Grid.SetRow(_listQuest, 1);
            Grid.SetColumn(_listQuest, 0);

            //Notation question
            TextBlock infoQuestionValue = new TextBlock();
            infoQuestionValue.Text = "Question : ";
            infoQuestionValue.Width = 75;
            infoQuestionValue.Foreground = new SolidColorBrush(Colors.White);
            Grid.SetRow(infoQuestionValue, 0);
            Grid.SetColumn(infoQuestionValue, 1);

            _questionValue = new TextBlock();
            _questionValue.Text = "No question selected.";
            _questionValue.Foreground = new SolidColorBrush(Colors.Red);
            Grid.SetRow(_questionValue, 0);
            Grid.SetColumn(_questionValue, 2);

            TextBlock infoNotation= new TextBlock();
            infoNotation.Text = "Notation : ";
            infoNotation.Foreground = new SolidColorBrush(Colors.White);
            Grid.SetRow(infoNotation, 1);
            Grid.SetColumn(infoNotation, 1);

            _notationValue = new ComboBox();
            _notationValue.Width = 50;
            _notationValue.Items.Add("A");
            _notationValue.Items.Add("B");
            _notationValue.Items.Add("C");
            Grid.SetRow(_notationValue, 1);
            Grid.SetColumn(_notationValue, 2);

            Button valide_question = new Button();
            valide_question.Content = "Valide this notation";
            valide_question.Click += new System.Windows.RoutedEventHandler(notation_valide_question);
            Grid.SetRow(valide_question, 0);
            Grid.SetColumn(valide_question, 3);

            Button valide_all = new Button();
            valide_all.Content = "Finish question";
            valide_all.Click += new System.Windows.RoutedEventHandler(StartValidation);
            Grid.SetRow(valide_all, 1);
            Grid.SetColumn(valide_all, 3);

            dynamiqueGrid.Children.Add(infoQuestion);
            dynamiqueGrid.Children.Add(_listQuest);
            dynamiqueGrid.Children.Add(infoQuestionValue);
            dynamiqueGrid.Children.Add(_questionValue);
            dynamiqueGrid.Children.Add(infoNotation);
            dynamiqueGrid.Children.Add(_notationValue);
            dynamiqueGrid.Children.Add(valide_question);
            dynamiqueGrid.Children.Add(valide_all);

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Database1Entities1 data = new Database1Entities1();
            if (_listQuest.SelectedItem != null)
                _questionValue.Text = data.Questions.Find(Convert.ToInt32(Regex.Match(_listQuest.SelectedItem.ToString(), @"\d+").Value)).Question;
        }

        private void notation_valide_question(object sender, EventArgs e)
        {
            Database1Entities1 data = new Database1Entities1();
            if (_notationValue.SelectedItem != null && _listQuest.SelectedItem != null)
            {
                //check bonus or important
                if (data.Questions.Find(Convert.ToInt32(Regex.Match(_listQuest.SelectedItem.ToString(), @"\d+").Value)).Bonus == true)
                {
                    if (_notationValue.SelectedItem.ToString() == "A")
                        _allRanking += "AA";
                    else if (_notationValue.SelectedItem.ToString() == "B")
                        _allRanking += "A";
                    else
                        _allRanking += "C";
                }
                else
                {
                    if (_notationValue.SelectedItem.ToString() == "C")
                        _allRanking += "CC";
                    else
                        _allRanking += _notationValue.SelectedItem.ToString();
                }

                _listQuest.Items.Remove(_listQuest.SelectedItem);

                TextBlock info = new TextBlock();
                info.Text = "Succes notation.";
                info.Foreground = new SolidColorBrush(Colors.Black);
                info.Background = new SolidColorBrush(Colors.Green);
                Grid.SetRow(info, 2);
                Grid.SetColumn(info, 2);
                dynamiqueGrid.Children.Add(info);
            }
            
        }


        private void StartValidation(object sender, EventArgs e)
        {
            dynamiqueGrid.Children.Clear();

            //Initialisation
            Database1Entities1 data = new Database1Entities1();

            //notation certification
            if (data.Teacher.Find(_id).IdCertification != null)
            {
                if (data.Certification.Find(data.Teacher.Find(_id).IdCertification).Grade == "Professional")
                    _allRanking += 'A';
                else if (data.Certification.Find(data.Teacher.Find(_id).IdCertification).Grade == "Academic")
                    _allRanking += 'B';
                else
                    _allRanking += 'C';
            }
            else
            {
                _allRanking += 'C';
            }

            //Notation  ECTS Credits
            TextBlock infoEcts= new TextBlock();
            infoEcts.Text = "Validation of ECTS credits : ";
            infoEcts.Foreground = new SolidColorBrush(Colors.White);
            Grid.SetRow(infoEcts, 0);
            Grid.SetColumn(infoEcts, 0);


            _inputEcts = new TextBox();
            _inputEcts.Text = "RESIT";
            Grid.SetRow(_inputEcts, 0);
            Grid.SetColumn(_inputEcts, 1);

            //Notation work experience
            TextBlock infoWork = new TextBlock();
            infoWork.Text = "For work experience : ";
            infoWork.Foreground = new SolidColorBrush(Colors.White);
            Grid.SetRow(infoWork, 1);
            Grid.SetColumn(infoWork, 0);


            _notationValue = new ComboBox();
            _notationValue.Width = 50;
            _notationValue.Items.Add("A");
            _notationValue.Items.Add("B");
            _notationValue.Items.Add("C");
            Grid.SetRow(_notationValue, 1);
            Grid.SetColumn(_notationValue, 1);

            
            Button validation_start = new Button();
            validation_start.Content = "Finish";
            validation_start.Click += new System.Windows.RoutedEventHandler(validation_start_click);
            Grid.SetRow(validation_start, 2);
            Grid.SetColumn(validation_start, 1);

            dynamiqueGrid.Children.Add(infoEcts);
            dynamiqueGrid.Children.Add(_inputEcts);
            dynamiqueGrid.Children.Add(infoWork);
            dynamiqueGrid.Children.Add(_notationValue);
            dynamiqueGrid.Children.Add(validation_start);

        }


        private void validation_start_click(object sender, EventArgs e)
        {
            Database1Entities1 data = new Database1Entities1();

            string ects = null;

            ects = _inputEcts.Text;
            if (ects == "RESIT")
                _allRanking += 'C';
            else
            {
                if (Convert.ToInt32(Regex.Match(ects, @"\d+").Value) >= 15)
                    _allRanking += 'A';
                else if (Convert.ToInt32(Regex.Match(ects, @"\d+").Value) >= 10)
                    _allRanking += 'B';
                else
                    _allRanking += 'C';
            }

            if (_notationValue.SelectedItem != null)
                _allRanking += _notationValue.SelectedItem.ToString();
            else
                _allRanking += 'C';

            if (Validate(_allRanking))
            {
                dynamiqueGrid.Children.Clear();

                TextBlock info = new TextBlock();
                info.Text = "The teacher is validate !";
                info.Foreground = new SolidColorBrush(Colors.Black);
                info.Background = new SolidColorBrush(Colors.Green);
                Grid.SetRow(info, 0);
                Grid.SetColumn(info, 0);
                dynamiqueGrid.Children.Add(info);

                SendMail(data.Teacher.Find(_id));

            }
            else
            {
                dynamiqueGrid.Children.Clear();

                TextBlock info = new TextBlock();
                info.Text = "The teacher is not valide.";
                info.Foreground = new SolidColorBrush(Colors.Red);
                Grid.SetRow(info, 0);
                Grid.SetColumn(info, 0);
                dynamiqueGrid.Children.Add(info);
            }
        }

        private Boolean Validate(string notation)
        {
            int nbA = 0;
            int nbC = 0;
            Boolean validate = false;

            for (int i = 0; i < notation.Length; i++)
            {
                if (notation[i] == 'A')
                    nbA++;
                else if (notation[i] == 'C')
                    nbC++;
            }

            if (nbC == 0 && nbA >= 1)
                validate = true;
            else if (nbC == 1 && nbA >= 2)
                validate = true;

            return validate;
        }

        private void SendMail(Teacher teacherValidate)
        {
            string signature = "The Full Prof of " + _course;
            _mailT = teacherValidate.Email;

            TextBlock infoMail = new TextBlock();
            infoMail.Text = "Mail d'envois : ";
            infoMail.Foreground = new SolidColorBrush(Colors.White);
            Grid.SetRow(infoMail, 1);
            Grid.SetColumn(infoMail, 0);
            dynamiqueGrid.Children.Add(infoMail);

            _mail = new TextBox();
            _mail.Text = "fullProf@supinfo.com";
            Grid.SetRow(_mail, 1);
            Grid.SetColumn(_mail, 1);
            dynamiqueGrid.Children.Add(_mail);

            TextBlock infoPassword = new TextBlock();
            infoPassword.Text = "Le mot de passe : ";
            infoPassword.Foreground = new SolidColorBrush(Colors.White);
            Grid.SetRow(infoPassword, 2);
            Grid.SetColumn(infoPassword, 0);
            dynamiqueGrid.Children.Add(infoPassword);

            _password = new PasswordBox();
            Grid.SetRow(_password, 2);
            Grid.SetColumn(_password, 1);
            dynamiqueGrid.Children.Add(_password);

            TextBlock infoIp = new TextBlock();
            infoIp.Text = "L'adresse IP ou le nom du serveur smtp : ";
            infoIp.Foreground = new SolidColorBrush(Colors.White);
            Grid.SetRow(infoIp, 3);
            Grid.SetColumn(infoIp, 0);
            dynamiqueGrid.Children.Add(infoIp);

            _ip = new TextBox();
            _ip.Text = "127.0.0.1";
            Grid.SetRow(_ip, 3);
            Grid.SetColumn(_ip, 1);
            dynamiqueGrid.Children.Add(_ip);

            TextBlock infoPort = new TextBlock();
            infoPort.Text = "Le port d'utilisation : ";
            infoPort.Foreground = new SolidColorBrush(Colors.White);
            Grid.SetRow(infoPort, 4);
            Grid.SetColumn(infoPort, 0);
            dynamiqueGrid.Children.Add(infoPort);

            _port = new TextBox();
            _port.Text = "587";
            Grid.SetRow(_port, 4);
            Grid.SetColumn(_port, 1);
            dynamiqueGrid.Children.Add(_port);

            TextBlock infoSubject = new TextBlock();
            infoSubject.Text = "Subject : ";
            infoSubject.Foreground = new SolidColorBrush(Colors.White);
            infoSubject.Margin = new Thickness(0, 50, 0, 0);
            Grid.SetRow(infoSubject, 5);
            Grid.SetColumn(infoSubject, 0);
            dynamiqueGrid.Children.Add(infoSubject);

            _subject = new TextBox();
            _subject.Text = "[ECTS " + _course + "] : Validation obtained";
            _subject.Margin = new Thickness(0, 50, 0, 0);
            Grid.SetRow(_subject, 5);
            Grid.SetColumn(_subject, 1);
            dynamiqueGrid.Children.Add(_subject);

            TextBlock infoBody = new TextBlock();
            infoBody.Text = "Body : ";
            infoBody.Foreground = new SolidColorBrush(Colors.White);
            Grid.SetRow(infoBody, 6);
            Grid.SetColumn(infoBody, 0);
            dynamiqueGrid.Children.Add(infoBody);

            _body = new TextBox();
            _body.Text = "You are now a teacher.\n" + signature;
            _body.Height = 300;
            _body.Width = 300;
            _body.AcceptsReturn = true;
            Grid.SetRow(_body, 6);
            Grid.SetColumn(_body, 1);
            dynamiqueGrid.Children.Add(_body);

            Button sendButton = new Button();
            sendButton.Content = "Send mail";
            sendButton.Width = 100;
            sendButton.Click += new System.Windows.RoutedEventHandler(sendMail_button);
            Grid.SetRow(sendButton, 7);
            Grid.SetColumn(sendButton, 1);
            dynamiqueGrid.Children.Add(sendButton);

        }

        private void sendMail_button(object sender, EventArgs e)
        {
            string fromMail, mdp, server;
            int port = 587;

            fromMail = _mail.Text;

            mdp = _password.Password.ToString();

            server = _ip.Text;

            port = Convert.ToInt32(Regex.Match(_port.Text, @"\d+").Value);

            try
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                message.To.Add(_mailT);
                message.Subject = _subject.Text;
                message.From = new System.Net.Mail.MailAddress(fromMail);
                message.Body = _body.Text;
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(server, port);

                smtp.Credentials = new System.Net.NetworkCredential(fromMail, mdp);

                smtp.EnableSsl = true;

                smtp.Send(message);

                dynamiqueGrid.Children.Clear();

                TextBlock info = new TextBlock();
                info.Text = "Succes sending mail.";
                info.Foreground = new SolidColorBrush(Colors.Black);
                info.Background = new SolidColorBrush(Colors.Green);
                Grid.SetRow(info, 0);
                Grid.SetColumn(info, 0);
                dynamiqueGrid.Children.Add(info);
                
            }
            catch (Exception exception)
            {
                TextBlock info = new TextBlock();
                info.Text = "FAIL sending mail.";
                info.Foreground = new SolidColorBrush(Colors.Black);
                info.Background = new SolidColorBrush(Colors.Red);
                Grid.SetRow(info, 0);
                Grid.SetColumn(info, 0);
                dynamiqueGrid.Children.Add(info);

                Console.Out.WriteLine("Erreur : " + exception);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //take id
            int id = Convert.ToInt32(Regex.Match(id_enter_input.Text, @"\d+").Value);

            selectId(id);
        }
    }
}
