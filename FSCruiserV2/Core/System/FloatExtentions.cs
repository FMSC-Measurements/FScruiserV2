namespace System
{
    public static class FloatExtentions
    {
        public static bool EqualsEx(this float x, float y)
        {
            return x.EqualsEx(y, .000001f);
        }

        public static bool EqualsEx(this float x, float y, float epsilon)
        {
            if (x == y) { return true; }
           
            // Handle NaN, Infinity.
            if (float.IsInfinity(x) | float.IsNaN(x))
                return x.Equals(y);
            else if (float.IsInfinity(y) | float.IsNaN(y))
                return x.Equals(y);
            else 
            {
                return Math.Abs(x - y) < epsilon;
            }
        }

        public static bool EqualsEx(this float x, float? y)
        {
            if (y == null) { return false; }
            return x.EqualsEx(y.Value, .000001f);
        }

        public static bool EqualsEx(this float x, float? y, float epsilon)
        {
            if (y == null) { return false; }
            return x.EqualsEx(y.Value, epsilon);
        }

        public static bool EqualsEx(this float? x, float y)
        {
            
            if (x == null) { return false; }
            return x.Value.EqualsEx(y, .000001f);
        }

        public static bool EqualsEx(this float? x, float y, float epsilon)
        {
            if (x == null) { return false; }
            return x.Value.EqualsEx(y, epsilon);
        }

        public static bool EqualsEx(this float? x, float? y)
        {
            if (x == null && y == null) { return true; }
            else if (x != null)
            {
                return x.Value.EqualsEx(y, .000001f);
            }
            else
            {
                return y.Value.EqualsEx(x, .000001f);
            }
        }

        public static bool EqualsEx(this float? x, float? y, float epsilon)
        {
            if (x == null && y == null) { return true; }
            else if (x != null)
            {
                return x.Value.EqualsEx(y, epsilon);
            }
            else
            {
                return y.Value.EqualsEx(x, epsilon);
            }
        }

    }
}
