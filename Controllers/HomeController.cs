using Events_asp.Database;
using Events_asp.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Events_asp.Controllers
{
    public class HomeController : Controller
    {
        public static int current_user_id {  get; set; }

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Index() => View();
        
        [HttpPost]
        public IActionResult Index(string name_ , DateTime date) {
           

            using (var db = new AppDBContext())
            {
                DateTime date_to_add = date.Date;
                DateTime time_to_add = date;

                //Вывести пользователя
                User current_user_db = db.users.Where(x => x.UserId == current_user_id).FirstOrDefault();
                db.SaveChanges();

                //Проверка есть ли эта дата в базе данных
                bool isDateAlreadyInDb = db.event_dates.Select(x => x.DateTime_).Contains(date_to_add);
               
                
                //Действия, если дата есть в бд
                if (isDateAlreadyInDb)
                {
                    EventDate eventDateToAdd = db.event_dates.Where(x => x.DateTime_ == date_to_add).FirstOrDefault();
                    int idDateToAlter = db.event_dates.Where(x => x.DateTime_ == date_to_add).Select(x => x.EventDateId).FirstOrDefault();
                    db.event_times.Add(new EventTime() { Name = name_, EventDate = eventDateToAdd, DateTime_ = date });


                    db.SaveChanges();


                }

                //Действия, если даты нет в бд
                else
                {
                  

                    db.event_dates.Add(new EventDate() { DateTime_ = date_to_add, User = current_user_db });
                     db.SaveChanges();
                    EventDate eventDateToAlter = db.event_dates.Where(x => x.DateTime_ == date_to_add).FirstOrDefault();
                    db.event_times.Add(new EventTime() { Name = name_, EventDate = eventDateToAlter, DateTime_ = date });


                    db.SaveChanges();
                }



            }
            return RedirectToAction("Index", "Home");

        }//$"Название: {name_}. Дата: {date}";

        //Вывод всех Событий
        [HttpGet]
        public IActionResult AllEvents() {
            EventViewModel eventViewModel = new EventViewModel();
            using (AppDBContext db = new AppDBContext())
            {
               

                //Получение списка из Событий из базы данных для общего вывода
                eventViewModel.eventDates = db.event_dates.Where(x => x.UserId == current_user_id).OrderBy(x => x.DateTime_).ToList();
                eventViewModel.eventTimes = db.event_times.Where(x => x.EventDate.UserId == current_user_id).OrderBy(x => x.DateTime_).ToList();

                db.SaveChanges();
            }

            return View(eventViewModel); 
        }
        //Удаление ненужных или исполненных событий
        [HttpPost]
        public IActionResult AllEvents(List<string> deleting_string_list)
        {
            EventViewModel eventViewModel = new EventViewModel();
            using (AppDBContext db = new AppDBContext())
            {
                //Получение списков из строк, которые были выбраны как удалённые и Id собтий их дат
                
                List<string> deleting_string_list_string = new List<string>();
                List<int> deleting_string_list_int = new List<int>();
                List<string> deleting_string_list_date = new List<string>();

                //Парсинг полученных строчек и разбитие их по имени, идентификатора даты и дате
                for (int aa = 0; aa< deleting_string_list.Count; aa++)
                {

                    string[] strings = deleting_string_list[aa].Split("___;___");
                    deleting_string_list_string.Add(strings[0]);
                    deleting_string_list_int.Add(Convert.ToInt32(strings[1]));
                    deleting_string_list_date.Add(strings[2]);

                }


                List<EventTime> events_to_delete = new List<EventTime>();
                
                
                List<EventTime> all_events = db.event_times.Where(x => x.EventDate.UserId == current_user_id).ToList();
                foreach (var eventt in all_events)
                {
                    for(int bb = 0;bb< deleting_string_list_string.Count; bb++)
                    {
                        if(eventt.EventDateId == deleting_string_list_int[bb] && eventt.Name == deleting_string_list_string[bb] && eventt.DateTime_.ToString() == deleting_string_list_date[bb])
                        {
                            events_to_delete.Add(eventt);
                        }
                    }

                }
                //var idEmptyDateToDelete = db.event_times.Where(x => deleting_string_list_string.Contains(x.Name) && deleting_string_list_int.Contains(x.EventDateId)).Select(x => x.EventDateId).ToList();

                var idEmptyDateToDelete = events_to_delete.Select(x => x.EventDateId).ToList();
                //Удаление событий
                db.event_times.RemoveRange(events_to_delete);
                db.SaveChanges();

                //Проверка и удаление дат без событий
                List<EventDate> all_events_date = db.event_dates.ToList();
                var empty_dates = new List<EventDate>();
                foreach(var eventt in all_events_date)
                {
                    if(eventt.eventTimes == null)
                    {
                        empty_dates.Add(eventt);
                    }

                }

         
                db.event_dates.RemoveRange(empty_dates);
                db.SaveChanges();

                //Получение списка из событие по пользователю, для передачи в метод
                eventViewModel.eventDates = db.event_dates.Where(x => x.UserId == current_user_id).OrderBy(x => x.DateTime_).ToList();
                eventViewModel.eventTimes = db.event_times.Where(x => x.EventDate.UserId == current_user_id).OrderBy(x => x.DateTime_).ToList();

            }
            return View(eventViewModel);

        }
        //Выезд
        //public static void Delete_Event()
        //{

        //}


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
