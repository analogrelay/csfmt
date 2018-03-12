namespace Test
{
    public static class ControlFlowSpacing
    {
        public static void TestMethod() 
        {
            if(true)
            {
            }

            while(true)
            {
            }

            do
            {
            } while(true);

            lock(new object())
            {
            }

            fixed(char* foo = "foo")
            {
            }

            switch(foo)
            {
            }
        }
    }
}