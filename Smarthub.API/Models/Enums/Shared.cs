namespace Smarthub.API.Models.Enums
{
    public class Shared
    {
        public enum eOrderType
        {
            Normal = 1,

            Staff = 2,

            Mechanical = 3,

            Perishable = 4,

        }

        public enum eOrderStatus
        {
            New = 1,

            Processing = 2,

            Complete = 3,
        }

        public enum eProductType
        {
            Apparel = 1,
            Parts = 2,
            Equipment = 3,
            Motor = 4,
        }
    }
}
