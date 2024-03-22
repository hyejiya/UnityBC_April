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
        static void Main()
        {
            Sum(1, 2);
            Operations += Sum;

            Operations += (a, b) =>
            {
                return (a + b);
            };

            Barista barista1 = new Barista("James");

            Thread t1 = new Thread(() => //스레드 생성
            {
                HireBarista("James")
                .GoToWork()             
                .MakeRandomBeverage(); //chaining
            }, 1 * MB); //최대 스택크기를 지정해줄 수 있다.

            //스레드 이름 지정
            t1.Name = barista1.Name;
            //스레드 닫음
            t1.IsBackground = true;
            //start를 해줘야 CPU에서 실행된다.
            t1.Start();
            // 메인 스레드에서 호출함 -> t1을 모두 호출할 때까지 기다린 후 스레드 종료
            t1.Join(); 
            
            Thread.Sleep(1000); //1초 뒤에 프로세스가 종료됨

            ThreadPool.SetMinThreads(1, 0);
            ThreadPool.SetMaxThreads(4, 4);

            //==========================================================================//

            Task task1 = new Task(() =>
            {
                HireBarista("James")
                .GoToWork()
                .MakeRandomBeverage(); //chaining
            });//최대 스택크기를 지정해줄 수 없다 -> 알아서 해줌

            //start해줘야 실행됨
            task1.Start();

            // 메인 스레드에서 호출함 -> t1을 모두 호출할 때까지 기다린 후 스레드 종료
            task1.Wait();

            Task[] tasks = new Task[10];

            for(int i = 0;  i < tasks.Length; i++)
            {
                int index = i;
                tasks[i] = new Task(() =>
                {
                    HireBarista($"Barista {index}")
                    .GoToWork()
                    .MakeRandomBeverage();
                });
                tasks[i].Start();
            }

            
            Task.WaitAll(tasks); //모든 Task들이 끝날 때까지 기다린다.
            Task.WaitAny(tasks); //하나의 Task가 끝날 때까지 기다린다.


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
            _random = new Random();
        }

        public string Name { get; set; }
        private Random _random;

        public Barista GoToWork()
        {
            Console.WriteLine($"바리스타 {Name}은 출근합니다....");
            return this;
        }

        public Beverage MakeRandomBeverage()
        {
            Beverage beverage = (Beverage)_random.Next(1, Enum.GetValues(typeof(Beverage)).Length);
            Console.WriteLine($"바리스타 {Name}은 음료 {beverage}를 제조했습니다");
            return beverage;
        }


    }
}

