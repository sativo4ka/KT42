namespace KT42
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Имитация работы с ресурсами");
            Console.WriteLine();

            Console.WriteLine("1. Создание FakeResource");

            CreateFakeResources();

            Console.WriteLine("Объекты созданы, метод завершился.");
            Console.WriteLine("Финализаторы пока не запускались вручную.");
            Console.WriteLine();

            Console.WriteLine("Запускаем сборщик мусора...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            Console.WriteLine("Сборка мусора завершена.");
            Console.WriteLine();

            Console.WriteLine("2. Правильное освобождение через using");

            using (ManagedResource resource = new ManagedResource("Ресурс №1"))
            {
                Console.WriteLine("Ресурс используется внутри using.");
            }

            Console.WriteLine("Мы вышли из using.");
            Console.WriteLine();

            Console.WriteLine("3. Проверка повторного вызова Dispose");

            ManagedResource resource2 = new ManagedResource("Ресурс №2");

            resource2.Dispose();
            resource2.Dispose();
            resource2.Dispose();

            Console.WriteLine("Dispose вызван три раза, но сообщение появилось только один раз.");
            Console.WriteLine();

            Console.WriteLine("4. Создание ресурса без using");

            ManagedResource forgottenResource = new ManagedResource("Забытый ресурс");

            Console.WriteLine("Ресурс создан, Dispose не вызывался.");
            Console.WriteLine("Пока объект существует, мы не освобождаем его вручную.");

            forgottenResource = null;

            Console.WriteLine("Ссылка потеряна. Dispose всё ещё не вызывался.");
            Console.WriteLine();

            Console.WriteLine("Запускаем GC для забытого ресурса...");
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine("GC завершил работу.");
            Console.WriteLine("Но GC сам по себе не вызывает Dispose.");
            Console.WriteLine();

            Console.WriteLine("Эксперимент завершен.");
        }

        static void CreateFakeResources()
        {
            FakeResource resource1 = new FakeResource();
            FakeResource resource2 = new FakeResource();
            FakeResource resource3 = new FakeResource();
        }
    }

    class FakeResource
    {
        public FakeResource()
        {
            Console.WriteLine("FakeResource: открыт");
        }

        ~FakeResource()
        {
            Console.WriteLine("FakeResource: финализатор");
        }
    }

    class ManagedResource : IDisposable
    {
        private bool disposed;
        private string name;

        public ManagedResource(string name)
        {
            this.name = name;
            disposed = false;

            Console.WriteLine(name + ": открыт");
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            Console.WriteLine(name + ": Dispose вызван");

            disposed = true;
        }
    }
}
