using System;
using System.Collections.Generic;
using System.Text;

namespace Xero.ScreencastWeb.Models
{
    public class Phone
    {
        public string PhoneType { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneAreaCode { get; set; }
        public string PhoneCountryCode { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            string phoneString = string.Empty;

            if (!string.IsNullOrEmpty(PhoneCountryCode))
            {
                phoneString += string.Format("(+{0}) ", PhoneCountryCode);
            }

            if (!string.IsNullOrEmpty(PhoneAreaCode))
            {
                phoneString += PhoneAreaCode + " ";
            }

            if (!string.IsNullOrEmpty(PhoneNumber))
            {
                phoneString += PhoneAreaCode;
            }

            return phoneString;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is empty.
        /// </summary>
        /// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
        public bool IsEmpty
        {
            get { return (string.IsNullOrEmpty(PhoneNumber) && string.IsNullOrEmpty(PhoneAreaCode) && string.IsNullOrEmpty(PhoneCountryCode)); }
        }
    }

    public class Phones : List<Phone>
    {
    }
}
