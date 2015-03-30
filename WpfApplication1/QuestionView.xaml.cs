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
    /// Page Question
    public partial class QuestionView : Page
    {
        private string _question;
        private string _typeCourse;
        private Boolean _important;
        private TextBox _nameCourse;
        private int _actualID = 0;

        public QuestionView()
        {
            InitializeComponent();
        }

        public void Add()
        {
            gridCourses.Children.Clear();

            //Initialisation
            Database1Entities1 data = new Database1Entities1();

            _question = question.Text;
            if (important.SelectedValue != null)
                _important = (important.SelectedValue.ToString() == "True") ? true : false;
            else
                _important = true;

            if (cour.Text.Length >= 4)
                _typeCourse = cour.Text.Substring(0, 4);
            else
                _typeCourse = cour.Text;

            

            //run the adding table
            if (data.Courses.Find(_typeCourse) == null)
            {
                try
                {
                    TextBlock infoCourse = new TextBlock();
                    infoCourse.Text = "Course not found ! Name of the courses : ";
                    infoCourse.Foreground = new SolidColorBrush(Colors.Red);

                    _nameCourse = new TextBox();
                    _nameCourse.Text = "Name";

                    Button conf = new Button();
                    conf.Content = "Valide";
                    conf.Click += new System.Windows.RoutedEventHandler(valide_course); 

                    Grid.SetRow(infoCourse, 0);
                    Grid.SetColumn(infoCourse, 0);
                    Grid.SetRow(_nameCourse, 0);
                    Grid.SetColumn(_nameCourse, 1);
                    Grid.SetRow(conf, 0);
                    Grid.SetColumn(conf, 2);

                    gridCourses.Children.Add(infoCourse);
                    gridCourses.Children.Add(_nameCourse);
                    gridCourses.Children.Add(conf);

                }
                catch (Exception e)
                {
                    Console.Out.Write(e);
                }
            }

            try
            {
               data.Questions.Add(new Questions { Id = data.Questions.Count() + 1, CodeMat = _typeCourse, Question = _question, Bonus = _important });
                data.SaveChanges();

                TextBlock infoCourse = new TextBlock();
                infoCourse.Text = "Question create !";
                infoCourse.Foreground = new SolidColorBrush(Colors.Black);
                infoCourse.Background = new SolidColorBrush(Colors.Green);
                Grid.SetRow(infoCourse, 0);
                Grid.SetColumn(infoCourse, 1);
                gridCourses.Children.Add(infoCourse);

                //update id select
                _actualID = data.Questions.Count();

            }
            catch (Exception e)
            {
                Console.Out.Write(e);
            }

        }

        public void showList(string nameCourse)
        {
            //Initialisation
            Database1Entities1 data = new Database1Entities1();
            Boolean FindSomething = false;

            //launch the recherche
            foreach (Questions source in data.Questions.ToArray())
            {
                if (source.CodeMat == nameCourse)
                {
                    list.Items.Add("Id de la question : " + source.Id + " The question : " + source.Question + '\n' + "Bonus : " + ((source.Bonus == true) ? "False" : "True"));

                    FindSomething = true;
                }
            }
            if (FindSomething == false)
                list.Items.Add("Aucun résultat trouver.");

        }

        public void showList()
        {
            //Initialisation
            Database1Entities1 data = new Database1Entities1();
            Boolean FindSomething = false;

            //launch the recherche
            foreach (Questions source in data.Questions.ToArray())
            {
                list.Items.Add("Id de la question : " + source.Id + " The question : " + source.Question );

                FindSomething = true;
            }
            if (FindSomething == false)
                list.Items.Add("Aucun résultat trouver !");

        }
        


        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (list != null)
            {
                list.Items.Clear();
                if (inputCourse.Text == "*")
                    showList();
                else
                    showList(inputCourse.Text);
            }
            
        }


        private void list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (list.SelectedItem != null )
            {
                if(list.SelectedItem.ToString() !=  "Aucun résultat trouver.")
                {
                    //take id
                    int id = Convert.ToInt32(Regex.Match(list.SelectedItem.ToString(), @"\d+").Value);

                    //search in DB and display information
                    Database1Entities1 data = new Database1Entities1();
                    Questions quest = null;

                    quest = data.Questions.Find(id);

                    if (quest != null)
                    {
                        question.Text = quest.Question;
                        important.SelectedIndex = (quest.Bonus == true) ? 0 : 1;
                        cour.Text = quest.CodeMat;

                        //update the actual id
                        _actualID = id;
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Add();
        }

        private void valide_course(object sender, EventArgs e)
        {

            //Initialisation
            Database1Entities1 data = new Database1Entities1();

            gridCourses.Children.Clear();

            data.Courses.Add(new Courses { CodeMat = _typeCourse, Name = _nameCourse.Text.ToString() });
            data.SaveChanges();

            data.Questions.Add(new Questions { Id = data.Questions.Count() + 1, CodeMat = _typeCourse, Question = _question, Bonus = _important });
            data.SaveChanges();

            TextBlock infoCourse = new TextBlock();
            infoCourse.Text = "Question create !";
            infoCourse.Foreground = new SolidColorBrush(Colors.Black);
            infoCourse.Background = new SolidColorBrush(Colors.Green);
            Grid.SetRow(infoCourse, 0);
            Grid.SetColumn(infoCourse, 1);
            gridCourses.Children.Add(infoCourse);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            update();
        }

        private void update()
        {
            gridCourses.Children.Clear();

            //Initialisation
            Database1Entities1 data = new Database1Entities1();

            _question = question.Text;
            if (important.SelectedValue != null)
                _important = (important.SelectedValue.ToString() == "True") ? true : false;
            else
                _important = true;

            if (cour.Text.Length >= 4)
                _typeCourse = cour.Text.Substring(0, 4);
            else
                _typeCourse = cour.Text;



            //run the adding table
            if (data.Courses.Find(_typeCourse) == null && data.Questions.Find(_actualID) != null)
            {
                try
                {
                    TextBlock infoCourse = new TextBlock();
                    infoCourse.Text = "Course not found ! Name of the courses : ";
                    infoCourse.Foreground = new SolidColorBrush(Colors.Red);

                    _nameCourse = new TextBox();
                    _nameCourse.Text = "Name";

                    Button conf = new Button();
                    conf.Content = "Valide";
                    conf.Click += new System.Windows.RoutedEventHandler(valide_course);

                    Grid.SetRow(infoCourse, 0);
                    Grid.SetColumn(infoCourse, 0);
                    Grid.SetRow(_nameCourse, 0);
                    Grid.SetColumn(_nameCourse, 1);
                    Grid.SetRow(conf, 0);
                    Grid.SetColumn(conf, 2);

                    gridCourses.Children.Add(infoCourse);
                    gridCourses.Children.Add(_nameCourse);
                    gridCourses.Children.Add(conf);

                }
                catch (Exception e)
                {
                    Console.Out.Write(e);
                }
            }

            try
            {
                if (data.Questions.Find(_actualID) != null)
                {
                    Questions questUpdate = data.Questions.First(i => i.Id == _actualID);

                    questUpdate.Id = _actualID;
                    questUpdate.CodeMat = _typeCourse;
                    questUpdate.Question = _question;
                    questUpdate.Bonus = _important;

                    data.SaveChanges();

                    TextBlock infoCourse = new TextBlock();
                    infoCourse.Text = "Question update !";
                    infoCourse.Foreground = new SolidColorBrush(Colors.Black);
                    infoCourse.Background = new SolidColorBrush(Colors.Green);
                    Grid.SetRow(infoCourse, 0);
                    Grid.SetColumn(infoCourse, 1);
                    gridCourses.Children.Add(infoCourse);
                }
                else
                {
                    TextBlock infoCourse = new TextBlock();
                    infoCourse.Text = "Question not found !";
                    infoCourse.Foreground = new SolidColorBrush(Colors.Red);
                    Grid.SetRow(infoCourse, 0);
                    Grid.SetColumn(infoCourse, 1);
                    gridCourses.Children.Add(infoCourse);
                }
                

            }
            catch (Exception e)
            {
                Console.Out.Write(e);
            }
        }
 
    }
}
