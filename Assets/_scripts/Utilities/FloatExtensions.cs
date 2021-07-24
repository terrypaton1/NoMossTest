namespace AlCaTrAzzGames.Utilities
{
    public static class FloatExtensions
    {
        public static float Remap(this float from, float fromMin, float fromMax, float toMin, float toMax)
        {
            var fromAbs = from - fromMin;
            var fromMaxAbs = fromMax - fromMin;

            var normal = fromAbs / fromMaxAbs;

            var toMaxAbs = toMax - toMin;
            var toAbs = toMaxAbs * normal;

            var to = toAbs + toMin;

            return to;
        }

        //format a float (in seconds) to a readable string
        public static string FormatTime(float timeInSeconds)
        {
            int minutes = (int)timeInSeconds / 60;
            int seconds = (int)timeInSeconds - 60 * minutes;
            int milliseconds = (int)(1000 * (timeInSeconds - minutes * 60 - seconds));
            return string.Format("{0:0}:{1:00}.{2:000}", minutes, seconds, milliseconds);
        }
    }
}