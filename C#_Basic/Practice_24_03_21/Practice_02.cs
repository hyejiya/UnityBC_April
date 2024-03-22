namespace MultiThreading
{
    
    internal class Practice_02
    {
        const int MB = 1024 * 1024;

        public static Func<int, int, int> Operations;

        public static int Sum(int a, int b)
        {
            return a + b;
        }
        public void Main()
        {
            Sum(1, 2);
            Operations += Sum;

            Operations += (a, b) =>
            {
                return (a + b);
            };

            Thread t1 = new Thread(() => //스레드 생성
            {
                HireBarista("James").GoToWork();
            }, 1 * MB); //최대 스택크기를 지정해줄 수 있다.

            t1.Start(); //start를 해줘야 CPU에서 실행된다.
        }

        static Barista HireBarista(string nickname)
        {
            return new Barista(nickname);
        }
    }
    public class Barista
    {
        public enum Beverage
        {
            None,
            Aspresso,
            Latte,
            Lemonade
        }
        public Barista(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        private Random _random;

        public void GoToWork()
        {
            Console.WriteLine($"바리스타 {Name}은 출근합니다....");
        }

        public Beverage MakeRandomBeverage()
        {
            return (Beverage)_random.Next(1, Enum.GetValues(typeof(Beverage)).Length);
        }
    }
}

