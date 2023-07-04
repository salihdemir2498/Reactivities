namespace Application.Core
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public T Value { get; set; }
        public string Error { get; set; }
        public static Result<T> Success(T value) => new Result<T> { IsSuccess = true,Value = value };
        public static Result<T> Failure(string error) => new Result<T> { IsSuccess=false, Error=error };

    }
}

//Success static metodu, başarılı bir sonuç döndürmek için kullanılır. Bu metot, Result<T> türünden bir nesne oluşturur ve IsSuccess özelliğini true olarak ayarlar, Value özelliğine belirtilen değeri atar.
//Failure static metodu, başarısız bir sonuç döndürmek için kullanılır. Bu metot, Result<T> türünden bir nesne oluşturur ve IsSuccess özelliğini false olarak ayarlar, Error özelliğine belirtilen hatayı atar.
//Bu Result<T> sınıfı, genel olarak işlemlerin sonuçlarını ifade etmek için kullanılır. İşlemler başarılı ise Result<T>.Success metoduyla başarılı bir sonuç döndürülürken, işlemler başarısız ise Result<T>.Failure metoduyla başarısız bir sonuç döndürülür. Bu sayede, işlem sonuçları daha kolay ve tutarlı bir şekilde yönetilebilir.