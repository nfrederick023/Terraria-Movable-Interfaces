using System.Reflection;
using Terraria;
using static Terraria.Main;


namespace AccessMain
{
    static class AccessMainClass
    {
        public static object callMainMethod(string methodName, params object[] args)
        {
            var mainMethod = instance.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            if (mainMethod != null)
            {
                return mainMethod.Invoke(instance, args);
            }
            return null;
        }

        public static object getMainField(string fieldName)
        {

            var mainFieldNonStatic = instance.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (mainFieldNonStatic == null)
            {
                var mainFieldStatic = instance.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
                if (mainFieldStatic == null)
                {
                    return null;
                }
                object fieldValue = mainFieldStatic.GetValue(instance);

                if (fieldValue == null)
                {
                    return null;
                }

                return fieldValue;
            }
            else
            {
                object fieldValue = mainFieldNonStatic.GetValue(instance);

                if (fieldValue == null)
                {
                    return null;
                }

                return fieldValue;
            }
        }

        public static void setMainField(string fieldName, object value)
        {

            var mainFieldNonStatic = instance.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (mainFieldNonStatic == null)
            {
                var mainFieldStatic = instance.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
                if (mainFieldStatic != null)
                {
                    mainFieldStatic.SetValue(instance, value);
                }
            }
            else
            {
                mainFieldNonStatic.SetValue(instance, value);
            }
        }

        public static object getRecipeField(Recipe obj, string fieldName)
        {

            var mainFieldNonStatic = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (mainFieldNonStatic == null)
            {
                var mainFieldStatic = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Static);
                if (mainFieldStatic == null)
                {
                    return null;
                }
                object fieldValue = mainFieldStatic.GetValue(obj);

                if (fieldValue == null)
                {
                    return null;
                }

                return fieldValue;
            }
            else
            {
                object fieldValue = mainFieldNonStatic.GetValue(obj);

                if (fieldValue == null)
                {
                    return null;
                }

                return fieldValue;
            }
        }

    }
}