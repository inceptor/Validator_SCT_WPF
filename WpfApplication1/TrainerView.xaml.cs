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
    /// Page Trainer
    public partial class Page2 : Page
    {

        private int _id;
        private string _name = "";
        private string _firstName = "";
        private string _promotionValidation = "";
        private string _currentPromotion = "";
        private string _email = "";
        private string _campus = "";
        private string _coursesWould = "";
        private string _coursesTeach = "";
        private string _campusWould = "";
        private string _campusCourses = "";
        private string _certification = "";
        private string _comment = "";
        private string _grade = "";
        private TextBox _nameCourseWould = null;
        private TextBox _nameCourseTeach = null;
        private int _actualID = 164253;

        public Page2()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            add();
        }

        public void add()
        {
            gridCourses.Children.Clear();

            if (courseTeach.Text != "" && courseTeach.Text.Length>=4)
                courseTeach.Text = courseTeach.Text.Substring(0, 4);

            if (courseWould.Text != "" && courseTeach.Text.Length >= 4)
                courseWould.Text = courseWould.Text.Substring(0, 4);

            _id = Convert.ToInt32(Regex.Match(id.Text, @"\d+").Value);
            _name = lastName.Text;
            _firstName = firstname.Text;
            _promotionValidation = promotionDuring.Text;
            _currentPromotion = promotionCurrent.Text;
            _email = email.Text;
            _campus = campus.Text;
            _coursesWould = courseWould.Text;
            _coursesTeach = courseTeach.Text;
            _campusWould = campusWould.Text;
            _campusCourses = campusTeach.Text;
            _certification = certificationName.Text;
            _comment = comment.Text;

            if (certificationGrade.SelectedItem != null)
                _grade = Regex.Match(certificationGrade.SelectedItem.ToString(), @"\w$").Value;
            else
                _grade = "None";

            Database1Entities1 data = new Database1Entities1();

            if (CheckForeignKey(data,"create") == true)
            {
                data.Teacher.Add(new Teacher
                {
                    Id = _id,
                    Name = _name,
                    Firstname = _firstName,
                    PromotionCurrent = _currentPromotion,
                    PromotionValidation = _promotionValidation,
                    Email = _email,
                    IdCoursesTeach = (_coursesTeach == "") ? null : _coursesTeach,
                    IdCoursesWould = _coursesWould,
                    IdCampusTeach = (_campusCourses == "") ? (int?)null : FindCampusByName(data, _campusCourses),
                    IdCampusWould = FindCampusByName(data, _campusWould),
                    IdCampus = FindCampusByName(data, _campus),
                    IdCertification = (_certification == "") ? (int?)null : FindCertificationByNameAndGrade(data, _certification, _grade),
                    Comments = _comment
                });
                data.SaveChanges();

                TextBlock infoCourse = new TextBlock();
                infoCourse.Text = "Teacher create !";
                infoCourse.Foreground = new SolidColorBrush(Colors.Black);
                infoCourse.Background = new SolidColorBrush(Colors.Green);
                Grid.SetRow(infoCourse, 0);
                Grid.SetColumn(infoCourse, 1);
                gridCourses.Children.Add(infoCourse);
            }

            
        }

        private Boolean CheckForeignKey(Database1Entities1 data,string flag)
        {
            Boolean find = false;
            Boolean check = true;

            //------------CHECK TEACHER PRIMARY KEY TABLE-----------

            if (data.Teacher.Find(_id) != null && flag !="update")
            {
                TextBlock infoCourse = new TextBlock();
                infoCourse.Text = "Duplicate ID, enter a new ID for this trainer !";
                infoCourse.Foreground = new SolidColorBrush(Colors.Red);
                Grid.SetRow(infoCourse, 2);
                Grid.SetColumn(infoCourse, 1);
                gridCourses.Children.Add(infoCourse);

                check = false;
            }

            //------------------------------------------------------

            //------------CHECK CAMPUS TABLE-----------
            //campus 
            foreach (Campus source in data.Campus.ToArray())
            {
                if (source.CampusName == _campus)
                    find = true;
            }

            if (!(find))
            {
                try
                {
                    data.Campus.Add(new Campus { Id = data.Campus.Count() + 1, CampusName = _campus });
                    data.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.Out.Write(e);
                }
            }

            //Campus would

            find = false;
            foreach (Campus source in data.Campus.ToArray())
            {
                if (source.CampusName == _campusWould)
                    find = true;
            }

            if (!(find))
            {
                try
                {
                    data.Campus.Add(new Campus { Id = data.Campus.Count() + 1, CampusName = _campusWould });
                    data.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.Out.Write(e);
                }
            }

            //campus teach

            if (_campusCourses != "")
            {
                find = false;
                foreach (Campus source in data.Campus.ToArray())
                {
                    if (source.CampusName == _campusCourses)
                        find = true;
                }

                if (!(find))
                {
                    try
                    {
                        data.Campus.Add(new Campus { Id = data.Campus.Count() + 1, CampusName = _campusCourses });
                        data.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        Console.Out.Write(e);
                    }
                }
            }

            //-----------------------------------------

            //------------CHECK COURSES TABLE-----------

            //courses teach
            if (_coursesTeach != "")
            {
                if (data.Courses.Find(_coursesTeach) == null)
                {
                    try
                    {
                        TextBlock infoCourse = new TextBlock();
                        infoCourse.Text = "Course not found ! Name of the courses Teach : ";
                        infoCourse.Foreground = new SolidColorBrush(Colors.Red);

                        _nameCourseTeach = new TextBox();
                        _nameCourseTeach.Text = "Name";

                        Button conf = new Button();
                        conf.Content = "Valide";
                        conf.Click += new System.Windows.RoutedEventHandler(valide_course);

                        Grid.SetRow(infoCourse, 0);
                        Grid.SetColumn(infoCourse, 0);
                        Grid.SetRow(_nameCourseTeach, 0);
                        Grid.SetColumn(_nameCourseTeach, 1);
                        Grid.SetRow(conf, 0);
                        Grid.SetColumn(conf, 2);

                        gridCourses.Children.Add(infoCourse);
                        gridCourses.Children.Add(_nameCourseTeach);
                        gridCourses.Children.Add(conf);

                        check = false;
                    }
                    catch (Exception e)
                    {
                        Console.Out.Write(e);
                    }
                }
            }


            //courses would teach
            if (data.Courses.Find(_coursesWould) == null)
            {
                try
                {
                    TextBlock infoCourse = new TextBlock();
                    infoCourse.Text = "Course not found ! Name of the courses Would : ";
                    infoCourse.Foreground = new SolidColorBrush(Colors.Red);

                    _nameCourseWould = new TextBox();
                    _nameCourseWould.Text = "Name";

                    Button conf = new Button();
                    conf.Content = "Valide";
                    conf.Click += new System.Windows.RoutedEventHandler(valide_course);

                    Grid.SetRow(infoCourse, 1);
                    Grid.SetColumn(infoCourse, 0);
                    Grid.SetRow(_nameCourseWould, 1);
                    Grid.SetColumn(_nameCourseWould, 1);
                    Grid.SetRow(conf, 1);
                    Grid.SetColumn(conf, 2);

                    gridCourses.Children.Add(infoCourse);
                    gridCourses.Children.Add(_nameCourseWould);
                    gridCourses.Children.Add(conf);

                    check = false;

                }
                catch (Exception e)
                {
                    Console.Out.Write(e);
                }
            }

            //------------------------------------------

            //----------------CHECK CERTIFICATION TABLE------------------

            if (_certification != "")
            {
                find = false;
                foreach (Certification source in data.Certification.ToArray())
                {
                    if (source.NameCertification == _certification && source.Grade == _grade)
                        find = true;
                }

                if (!(find))
                {
                    try
                    {
                        data.Certification.Add(new Certification { Id = data.Certification.Count() + 1, NameCertification = _certification, Grade = _grade });
                        data.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        Console.Out.Write(e);
                    }
                }
            }

            //------------------------------------------

            if(check == true)
                return true;
            else
                return false;

        }

        private int FindCampusByName(Database1Entities1 data, string name)
        {
            int value = 0;
            foreach (Campus source in data.Campus.ToArray())
            {
                if (source.CampusName == name)
                    value = source.Id;
            }

            return value;
        }

        private int FindCertificationByNameAndGrade(Database1Entities1 data, string name, string grade)
        {
            int value = 0;
            foreach (Certification source in data.Certification.ToArray())
            {
                if (source.NameCertification == name && source.Grade == grade)
                    value = source.Id;
            }

            return value;
        }

        private void valide_course(object sender, EventArgs e)
        {

            //Initialisation
            Database1Entities1 data = new Database1Entities1();

            gridCourses.Children.Clear();

            if (data.Courses.Find(_coursesTeach) == null && _nameCourseTeach != null )
            {
                data.Courses.Add(new Courses { CodeMat = _coursesTeach, Name = _nameCourseTeach.Text.ToString() });
                data.SaveChanges();
            }
            if (data.Courses.Find(_coursesWould) == null)
            {
                data.Courses.Add(new Courses { CodeMat = _coursesWould, Name = _nameCourseWould.Text.ToString() });
                data.SaveChanges();
            }

            add();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            findByID(Convert.ToInt32(Regex.Match(search.Text, @"\d+").Value));
        }

        private void findByID(int idSearch)
        {
            Database1Entities1 data = new Database1Entities1();
            Teacher teacher = null;

            teacher = data.Teacher.Find(idSearch);

            if (teacher != null)
            {
                id.Text = teacher.Id.ToString();

                lastName.Text = teacher.Name;

                firstname.Text = teacher.Firstname;

                promotionDuring.Text = teacher.PromotionValidation;

                promotionCurrent.Text = teacher.PromotionCurrent;

                email.Text = teacher.Email;

                campus.Text = data.Campus.Find(teacher.IdCampus).CampusName;

                courseWould.Text = teacher.IdCoursesWould;

                courseTeach.Text = teacher.IdCoursesTeach;

                campusTeach.Text = ((data.Campus.Find(teacher.IdCampusTeach) == null) ? "None." : data.Campus.Find(teacher.IdCampusTeach).CampusName);

                campusWould.Text = ((data.Campus.Find(teacher.IdCampusWould) == null) ? "None." : data.Campus.Find(teacher.IdCampusWould).CampusName);

                certificationName.Text = ((data.Certification.Find(teacher.IdCertification) == null) ? "None." : data.Certification.Find(teacher.IdCertification).NameCertification);

                if (data.Certification.Find(teacher.IdCampusWould) != null)
                {
                    switch (data.Certification.Find(teacher.IdCampusWould).Grade)
                    {
                        case "Professional":
                            certificationGrade.SelectedIndex = 0;
                            break;
                        case "Academic":
                            certificationGrade.SelectedIndex = 1;
                            break;
                        default :
                            certificationGrade.SelectedIndex = 2;
                            break;
                    }
                }

                comment.Text = teacher.Comments;
            }

        }

        private void id_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(email != null)
                email.Text = id.Text + "@supinfo.com";
        }

        private void update()
        {
            gridCourses.Children.Clear();

            if (courseTeach.Text != "" && courseTeach.Text.Length>=4)
                courseTeach.Text = courseTeach.Text.Substring(0, 4);

            if (courseWould.Text != "" && courseTeach.Text.Length >= 4)
                courseWould.Text = courseWould.Text.Substring(0, 4);

            _id = Convert.ToInt32(Regex.Match(id.Text, @"\d+").Value);
            _name = lastName.Text;
            _firstName = firstname.Text;
            _promotionValidation = promotionDuring.Text;
            _currentPromotion = promotionCurrent.Text;
            _email = email.Text;
            _campus = campus.Text;
            _coursesWould = courseWould.Text;
            _coursesTeach = courseTeach.Text;
            _campusWould = campusWould.Text;
            _campusCourses = campusTeach.Text;
            _certification = certificationName.Text;
            _comment = comment.Text;

            if (certificationGrade.SelectedItem != null)
                _grade = Regex.Match(certificationGrade.SelectedItem.ToString(), @"\w$").Value;
            else
                _grade = "None";

            Database1Entities1 data = new Database1Entities1();

            if (CheckForeignKey(data,"update") == true)
            {
                Teacher profUpdate = data.Teacher.First(i => i.Id == _id);

                profUpdate.Id = _id;
                profUpdate.Name = _name;
                profUpdate.Firstname = _firstName;
                profUpdate.PromotionCurrent = _currentPromotion;
                profUpdate.PromotionValidation = _promotionValidation;
                profUpdate.Email = _email;
                profUpdate.IdCoursesTeach = (_coursesTeach == "") ? null : _coursesTeach;
                profUpdate.IdCoursesWould = _coursesWould;
                profUpdate.IdCampusTeach = (_campusCourses == "") ? (int?) null : FindCampusByName(data, _campusCourses);
                profUpdate.IdCampusWould = FindCampusByName(data, _campusWould);
                profUpdate.IdCampus = FindCampusByName(data, _campus);
                profUpdate.IdCertification = (_certification == "") ? (int?) null : FindCertificationByNameAndGrade(data, _certification, _grade);
                profUpdate.Comments = _comment;

                data.SaveChanges();

                TextBlock infoCourse = new TextBlock();
                infoCourse.Text = "Teacher update !";
                infoCourse.Foreground = new SolidColorBrush(Colors.Black);
                infoCourse.Background = new SolidColorBrush(Colors.Green);
                Grid.SetRow(infoCourse, 0);
                Grid.SetColumn(infoCourse, 1);
                gridCourses.Children.Add(infoCourse);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            update();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Database1Entities1 data = new Database1Entities1();
            Boolean find = false;
            int index = 0,first = 0;

            foreach (Teacher teacher in data.Teacher.ToArray())
            {
                if (first == 0)
                    first = teacher.Id;
                
                if (teacher.Id == _actualID)
                {
                    find = true;
                }
                else if(find == true)
                {
                    index = teacher.Id;
                    find = false;
                }
                    
            }

            if (find == false && index != 0 && first !=0)
            {
                findByID(index);
                _actualID = index;
            }
            else
            {
                findByID(first);
                _actualID = first;
            }

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Database1Entities1 data = new Database1Entities1();
            Boolean find = false;
            int index = 0, first = 0;

            foreach (Teacher teacher in data.Teacher.ToArray())
            {
                if (first == 0)
                    first = teacher.Id;

                if (teacher.Id == _actualID)
                {
                    find = true;
                }
                else if(find == false)
                {
                    index = teacher.Id;
                }

            }

            if (find == true && first != 0)
            {
                findByID(index);
                _actualID = index;
            }
            else
            {
                findByID(first);
                _actualID = first;
            }
        }
    }

    
}
