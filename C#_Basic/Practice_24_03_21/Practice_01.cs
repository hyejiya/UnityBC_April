class CharacterController : object
{
    public float Hp
    {
        get => _hp;
        set => _hp  = value;
    }

    private float _hp;
}

struct ItemData
{
    public int Id;
    public int Num;
}

internal class Practice_01
{
    static void Main(string[] args) //stack : 4byte + 4byte
    {
        object obj = new CharacterController(); 
   
        obj = new ItemData { Id = 0, Num = 0 }; //boxing
        ItemData data = (ItemData)obj;//unboxing : 원래 값을 명시해줘야 함
        //Hip에 저장된 정보를 읽는 것을 unboxing이라고 한다.
        object obj_2 = obj;
        

        long a = 10;
        int b = (int)a;//작은 데이터에 큰 데이터를 넣는 건 명시적 형변환을 해줘야 함.
        a = b; //큰 데이터에 작은 데이터를 넣는 건 자동으로 변환됨

        StringWriter sw = new StringWriter();
        sw.Dispose();

        using (StreamWriter streamWriter = new StreamWriter(""))
        {

        }
    }
}