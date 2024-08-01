namespace Smarthub.API.Utility
{
    /// <summary>
    /// Helper class containing static utility methods
    /// </summary>
    public class Helper
    {
        /// <summary>
        /// Generates a new GUID
        /// </summary>
        /// <returns>A new GUID</returns>
        public static Guid GenerateGuid()
        {
            return Guid.NewGuid();
        }
    }
}