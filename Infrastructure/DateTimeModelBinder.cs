using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Events_asp.Infrastructure
{
    public class DateTimeModelBinder: IModelBinder
    {
        private readonly IModelBinder _binder;
        public DateTimeModelBinder(IModelBinder binder)
        {
            _binder = binder;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {

            var datePartValues = bindingContext.ValueProvider.GetValue("date_");
            var timePartValues = bindingContext.ValueProvider.GetValue("time_");

            // если не найдено значений с данными ключами, вызываем привязчик модели по умолчанию
            if (datePartValues == ValueProviderResult.None || timePartValues == ValueProviderResult.None)
                return _binder.BindModelAsync(bindingContext);

            // получаем значения
            string? date = datePartValues.FirstValue;
            string? time = timePartValues.FirstValue;

            // Парсим дату и время
            DateTime.TryParse(date, out var parsedDateValue);
            DateTime.TryParse(time, out var parsedTimeValue);

            // Объединяем полученные значения в один объект DateTime
            var result = new DateTime(parsedDateValue.Year,
                                                        parsedDateValue.Month,
                                        parsedDateValue.Day,
                                        parsedTimeValue.Hour,
                                        parsedTimeValue.Minute,
                                        parsedTimeValue.Second);

            // устанавливаем результат привязки
            bindingContext.Result = ModelBindingResult.Success(result);
            return Task.CompletedTask;



        }


    }
}
