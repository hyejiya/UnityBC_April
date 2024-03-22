internal class Async_
{
    //CancellationTokenSource
    //어떤 Task를 취소시키기 위해서 신호를 보내는 원본 객체참조
    //Task 할당 시에, 이 Source를 통해 Token을 발행해서 줄 수 있고,
    //이 Source의 Cancel() 요청이 발생했을 대, Token을 발행받는 모든 Task를 취소시킬 수 있다.

    
    public static int Ticks;

    static void Main(string[] args)
    {
        CancellationTokenSource cts = new CancellationTokenSource();

        Task t1 = Task.Factory.StartNew(() => HireBarista("Tom")
                                     .GoToWork()
                                     .MakeRandomBeverage()
         , cts.Token, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);

        cts.Cancel();

        if (t1.IsCanceled)
        {
            //todo
        }


        Task<Beverage> task = HireBarista("Tom")
              .GoToWork()
              .MakeRandomBeverage();

        task.Wait();

        Task[] tasks = new Task[10];
        for(int i = 0; i < 10; i++)
        {
            int index = i;
            tasks[index] = HireBarista("Tom")
                                     .GoToWork()
                                     .MakeRandomBeverage();
        }
        Task.WaitAll(tasks);

        Console.WriteLine($"Ticks : {Ticks}");
    }

    static Barista HireBarista(string nickname)
    {
        return new Barista(nickname);
    }

    public enum Beverage
    {
        None,
        Aspresso,
        Latte,
        Lemonade
    }

    public class Barista
    {


        public Barista(string name)
        {
            Name = name;
            _random = new Random();
        }

        public string Name { get; set; }

        private static Dictionary<Beverage, int> s_delayTimes = new Dictionary<Beverage, int>
        {
            { Beverage.Aspresso, 1000},
            { Beverage.Latte, 3000},
            { Beverage.Lemonade, 2000}
        };


        private Random _random;

        public Barista GoToWork()
        {
            Console.WriteLine($"바리스타 {Name}은 출근합니다....");
            return this;
        }

        public readonly static object Lock = new object(); //읽기전용 감시하려는 자물쇠 생성

        public async Task<Beverage> MakeRandomBeverage()
        {
            Beverage beverage = (Beverage)_random.Next(1, Enum.GetValues(typeof(Beverage)).Length);
            Console.WriteLine($"바리스타 {Name}은 음료 {beverage} 제조를 시작했습니다");

            await Task.Delay(s_delayTimes[beverage]);

            //lock 키워드 : 현재 어플리케이션 내에서 둘 이상의 스레드 접근을 막기위한 키워드
            lock(Lock)
            {
                for (int i = 0; i < 10_0000; i++)
                {
                    Async_.Ticks++;
                }
            }

            for(int i = 0; i < 10_0000; i++)
            {
                Interlocked.Increment(ref Async_.Ticks);
            }

            //감시 시작
            Monitor.Enter(Lock);

            //Critical Section(임계 영역) : 둘 이상의 스레드가 접근하면 안되는 공유 자원에 접근하는 영역
            //Critical Section 시작
            for (int i = 0; i < 100000; i++)
            {
                Async_.Ticks++;
            }
            //Critical Section 끝
            
            Monitor.Exit(Lock); //감시 끝

            Semaphore pool = new Semaphore(0, 3);
            pool.WaitOne(); //한 자리가 날 때까지 기다림
            //todo -> 크리티컬 섹션 작성
            pool.Release(); //점유하고 있는거 비움

            Mutex mutex = new Mutex();
            mutex.WaitOne();
            //todo -> 크리티컬 섹션 작성
            mutex.ReleaseMutex();



            Console.WriteLine($"바리스타 {Name}은 음료 {beverage} 제조를 완료했습니다");
            return beverage;
        }

        private Task Delay(int milliseconds)
        {
            return new Task(() =>
            {
                Thread.Sleep(milliseconds);
            });
        }
    }
}