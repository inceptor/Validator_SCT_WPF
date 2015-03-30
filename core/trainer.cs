using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace _164253
{
    class Trainer
    {
        private int _id;
        private string _name;
        private string _firstName;
        private string _promotionValidation;
        private string _currentPromotion;
        private string _email;
        private string _campus;
        private string _coursesWould;
        private string _coursesTeach;
        private string _campusWould;
        private string _campusCourses;
        private string _certification;
        private string _comment;
        private string _grade;


        public void add()
        {
            string tmp = "";

            Console.Out.WriteLine("------------ADDING A NEW TRAINER------------\n");

            Console.Out.Write("ID : ");
            _id = Convert.ToInt32(Console.In.ReadLine());

            Console.Out.Write("Name : ");
            _name = Console.In.ReadLine();

            Console.Out.Write("Firstname : ");
            _firstName = Console.In.ReadLine();

            Console.Out.Write("Promotion during validation : ");
            _promotionValidation = Console.In.ReadLine();

            Console.Out.Write("Current Promotion : ");
            _currentPromotion = Console.In.ReadLine();

            //Autogeneration of the email
            _email = Convert.ToString(_id) + "@supinfo.com";
            Console.Out.Write("Change this mail '"+_email+"' (emptyline if you don't change) : ");
            if( !( (tmp = Console.In.ReadLine() ) == "") )
                 _email = tmp;

            Console.Out.Write("Campus : ");
            _campus = Console.In.ReadLine();

            Console.Out.Write("Courses that he would teach : ");
            _coursesWould = Console.In.ReadLine();

            Console.Out.Write("Courses that he already teach : ");
            _coursesTeach = Console.In.ReadLine();

            //Resize the name of cursus for the autorisation lenght in the DB
            if (_coursesTeach!="")
                _coursesTeach = _coursesTeach.Substring(0, 4);
            if (_coursesWould!="")
                _coursesWould = _coursesWould.Substring(0, 4);

            Console.Out.Write("The campus on which he has already given a course : ");
            _campusWould = Console.In.ReadLine();

            Console.Out.Write("The campus on which he would give a course : ");
            _campusCourses = Console.In.ReadLine();

            Console.Out.Write("Certifications : ");
            _certification = Console.In.ReadLine();

            if (_certification != "")
            {
                Console.Out.Write("The grade of the certification only{Professional,Academic) : ");
                _grade = Console.In.ReadLine();
            }

            Console.Out.Write("Comments : ");
            _comment = Console.In.ReadLine();

            Database1Entities data = new Database1Entities();

            CheckForeignKey(data);

            data.Teacher.Add(new Teacher
            {
                Id = _id, Name = _name, 
                Firstname = _firstName, 
                PromotionCurrent = _currentPromotion, 
                PromotionValidation = _promotionValidation, 
                Email = _email, IdCoursesTeach = (_coursesTeach == "") ? null : _coursesTeach, 
                IdCoursesWould = _coursesWould, 
                IdCampusTeach = (_campusCourses == "") ? (int?)null : FindCampusByName(data, _campusCourses),
                IdCampusWould = FindCampusByName(data, _campusWould), IdCampus = FindCampusByName(data, _campus), 
                IdCertification = (_certification == "") ? (int?)null : FindCertificationByNameAndGrade(data,_certification,_grade), 
                Comments = _comment
            });
            data.SaveChanges();
        }

        private void CheckForeignKey(Database1Entities data)
        {
            Boolean find = false;

            //------------CHECK TEACHER PRIMARY KEY TABLE-----------

            while(data.Teacher.Find(_id)!=null)
            {
                Console.Out.Write("Duplicate ID, enter a new ID for this trainer :");
                _id = Convert.ToInt32(Console.In.ReadLine());
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
                Console.Out.WriteLine("Campus no found ! Create the campus...");
                try
                {
                    data.Campus.Add(new Campus {Id = data.Campus.Count()+1, CampusName = _campus});
                    data.SaveChanges();
                    Console.Out.WriteLine("Campus create !");
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
                Console.Out.WriteLine("Campus would teach no found ! Create the campus...");
                try
                {
                    data.Campus.Add(new Campus { Id = data.Campus.Count() + 1, CampusName = _campusWould });
                    data.SaveChanges();
                    Console.Out.WriteLine("Campus create !");
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
                    Console.Out.WriteLine("Campus teach no found ! Create the campus...");
                    try
                    {
                        data.Campus.Add(new Campus { Id = data.Campus.Count() + 1, CampusName = _campusCourses });
                        data.SaveChanges();
                        Console.Out.WriteLine("Campus create !");
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
                    Console.Out.WriteLine("Courses teach no found ! Create the courses...");
                    try
                    {
                        Console.Out.Write("Name of the courses : ");
                        data.Courses.Add(new Courses { CodeMat = _coursesTeach, Name = Console.In.ReadLine() });
                        data.SaveChanges();
                        Console.Out.WriteLine("Courses create !");
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
                Console.Out.WriteLine("Courses would teach no found ! Create the courses...");
                try
                {
                    Console.Out.Write("Name of the courses : ");
                    data.Courses.Add(new Courses {CodeMat = _coursesWould, Name = Console.In.ReadLine()});
                    data.SaveChanges();
                    Console.Out.WriteLine("Courses create !");
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
                    Console.Out.WriteLine("Certification no found ! Create the certification...");
                    try
                    {
                        data.Certification.Add(new Certification { Id = data.Certification.Count() + 1, NameCertification = _certification, Grade = _grade });
                        data.SaveChanges();
                        Console.Out.WriteLine("certification create !");
                    }
                    catch (Exception e)
                    {
                        Console.Out.Write(e);
                    }
                }
            }
            
            //------------------------------------------

        }

        private int FindCampusByName(Database1Entities data,string name)
        {
            int value = 0;
            foreach (Campus source in data.Campus.ToArray())
            {
                if (source.CampusName == name)
                    value = source.Id;
            }

            return value;
        }

        private int FindCertificationByNameAndGrade(Database1Entities data, string name,string grade)
        {
            int value = 0;
            foreach (Certification source in data.Certification.ToArray())
            {
                if (source.NameCertification == name && source.Grade==grade)
                    value = source.Id;
            }

            return value;
        }

        public void show()
        {
            //initialisation
            int idSearch = 0;
            string nameSearch = "";
            Database1Entities data = new Database1Entities();
            Teacher teacher = null;

            //Option de recherche
            Console.Out.Write("Search by ID(0)(by default) or Name(1) : ");
            if( Convert.ToInt32(Console.In.ReadLine())== 0)
            {
                Console.Out.Write("ID of the trainer : ");
                idSearch = Convert.ToInt32(Console.In.ReadLine());
                teacher = data.Teacher.Find(idSearch);
            }
            else
            {
                Console.Out.Write("Name of the trainer : ");
                nameSearch = Console.In.ReadLine();

                foreach (Teacher source in data.Teacher.ToArray())
                {
                    if (source.Name == nameSearch)
                        teacher = source;
                }
            }

            //color console
            Console.ForegroundColor = ConsoleColor.Red;

            if (teacher != null)
            {
                //Display information
                Console.Out.WriteLine("------INFORMATION TRAINER--------");
                Console.Out.WriteLine("ID : " + teacher.Id);

                Console.Out.WriteLine("Name : " + teacher.Name);

                Console.Out.WriteLine("Firstname : " + teacher.Firstname);

                Console.Out.WriteLine("Promotion during validation : " + teacher.PromotionValidation);

                Console.Out.WriteLine("Current Promotion : " + teacher.PromotionCurrent);

                Console.Out.WriteLine("email : " + teacher.Email);

                Console.Out.WriteLine("Campus : " + data.Campus.Find(teacher.IdCampus).CampusName);

                Console.Out.WriteLine("Courses that he would teach : " + teacher.IdCoursesWould);

                Console.Out.WriteLine("Courses that he already teach : " + teacher.IdCoursesTeach);

                Console.Out.WriteLine("The campus on which he has already given a course : " + ((data.Campus.Find(teacher.IdCampusTeach) == null) ? "None." : data.Campus.Find(teacher.IdCampusTeach).CampusName) );

                Console.Out.WriteLine("The campus on which he would give a course : " + ((data.Campus.Find(teacher.IdCampusWould) == null) ? "None." : data.Campus.Find(teacher.IdCampusWould).CampusName) );

                Console.Out.WriteLine("Certifications : " + ((data.Certification.Find(teacher.IdCertification) == null) ? "None." : data.Certification.Find(teacher.IdCertification).NameCertification));

                if (data.Certification.Find(teacher.IdCampusWould) != null)
                    Console.Out.WriteLine("\tThe grade of the certification: " + data.Certification.Find(teacher.IdCampusWould).Grade);

                Console.Out.WriteLine("Comments : " + teacher.Comments);
            }
            else
            {
                Console.Out.WriteLine("Trainer no found.");
            }

            //reset color
            Console.ResetColor();

        }
        
    }
}
