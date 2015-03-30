using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _164253
{
    class Question
    {
        private string _question;
        private string _typeCourse;
        private Boolean _important;

        public void Add()
        {
            //Initialisation
            Database1Entities data = new Database1Entities();

            Console.Out.Write("Question for the trainer :");
            _question = Console.In.ReadLine();

            Console.Out.Write("Bonus(0) or important(1) question :");
            _important = (Convert.ToInt32(Console.In.ReadLine()) == 1) ? true : false;

            Console.Out.Write("This question is for the course :");
            _typeCourse = Console.In.ReadLine();
            _typeCourse = _typeCourse.Substring(0, 4);

            //run the adding table
            if (data.Courses.Find(_typeCourse) == null)
            {
                Console.Out.WriteLine("Courses no found ! Create the courses...");
                try
                {
                    Console.Out.Write("Name of the courses : ");
                    data.Courses.Add(new Courses { CodeMat = _typeCourse, Name = Console.In.ReadLine() });
                    data.SaveChanges();
                    Console.Out.WriteLine("Courses create !");
                }
                catch (Exception e)
                {
                    Console.Out.Write(e);
                }
            }

            Console.Out.WriteLine("Create the question...");
            try
            {
                data.Questions.Add(new Questions{Id = data.Questions.Count() + 1,CodeMat = _typeCourse,Question = _question,Bonus = _important});
                data.SaveChanges();
                Console.Out.WriteLine("Question create !");
            }
            catch (Exception e)
            {
                Console.Out.Write(e);
            }

        }

        public void show(string nameCourse)
        {
            //Initialisation
            Database1Entities data = new Database1Entities();
            Boolean FindSomething = false;

            //launch the recherche
            foreach (Questions source in data.Questions.ToArray())
            {
                if (source.CodeMat == nameCourse)
                {
                    Console.Out.WriteLine("--------------------------------");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Out.WriteLine("ID of the question : "+source.Id);
                    Console.Out.WriteLine("The question : " + source.Question);
                    Console.Out.WriteLine("Bonus : " + ((source.Bonus==true) ? "False" : "True") );
                    Console.ResetColor();
                    Console.Out.WriteLine("--------------------------------");

                    FindSomething = true;
                }
            }
            if(FindSomething==false)
                Console.Out.WriteLine("Aucun résultat trouver !");
            
        }
        public void show()
        {
            //Initialisation
            Database1Entities data = new Database1Entities();
            Boolean FindSomething = false;

            //launch the recherche
            foreach (Questions source in data.Questions.ToArray())
            {
                Console.Out.WriteLine("--------------------------------");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine("ID of the question : " + source.Id);
                Console.Out.WriteLine("The question : " + source.Question);
                Console.Out.WriteLine("Bonus : " + ((source.Bonus == true) ? "False" : "True"));
                Console.Out.WriteLine("The course : " + source.CodeMat);
                Console.ResetColor();
                Console.Out.WriteLine("--------------------------------");

                    FindSomething = true;
            }
            if (FindSomething == false)
                Console.Out.WriteLine("Aucun résultat trouver !");

        }

    }
}
