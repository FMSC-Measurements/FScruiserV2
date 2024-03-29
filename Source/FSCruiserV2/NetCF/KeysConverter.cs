﻿namespace System.Windows.Forms
{
    using System.Text;

    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;
    
    /// <include file='doc\KeysConverter.uex' path='docs/doc[@for="KeysConverter"]/*' />
    /// <devdoc>
    /// <para>Provides a type converter to convert <see cref='System.Windows.Forms.Keys'/> objects to and from various 
    ///    other representations.</para>
    /// </devdoc>
    public class KeysConverter : TypeConverter, IComparer
    {
        private IDictionary keyNames;
        private List<string> displayOrder;

        private const Keys FirstDigit = System.Windows.Forms.Keys.D0;
        private const Keys LastDigit = System.Windows.Forms.Keys.D9;
        private const Keys FirstAscii = System.Windows.Forms.Keys.A;
        private const Keys LastAscii = System.Windows.Forms.Keys.Z;
        private const Keys FirstNumpadDigit = System.Windows.Forms.Keys.NumPad0;
        private const Keys LastNumpadDigit = System.Windows.Forms.Keys.NumPad9;

        private void AddKey(string key, Keys value)
        {
            keyNames[key] = value;
            displayOrder.Add(key);
        }

        private void Initialize()
        {
            keyNames = new Hashtable(34);
            displayOrder = new List<string>(34);

            AddKey("Enter", Keys.Return);
            AddKey("F12", Keys.F12);
            AddKey("F11", Keys.F11);
            AddKey("F10", Keys.F10);
            AddKey("End", Keys.End);
            AddKey("Ctrl", Keys.Control);
            AddKey("F8", Keys.F8);
            AddKey("F9", Keys.F9);
            AddKey("Alt", Keys.Alt);
            AddKey("F4", Keys.F4);
            AddKey("F5", Keys.F5);
            AddKey("F6", Keys.F6);
            AddKey("F7", Keys.F7);
            AddKey("F1", Keys.F1);
            AddKey("F2", Keys.F2);
            AddKey("F3", Keys.F3);
            AddKey("PgDn", Keys.Next);
            AddKey("Ins", Keys.Insert);
            AddKey("Home", Keys.Home);
            AddKey("Del", Keys.Delete);
            AddKey("Sft", Keys.Shift);
            AddKey("PgUp", Keys.Prior);
            AddKey("Back", Keys.Back);

            //new whidbey keys follow here...
            // VSWhidbey 95006: Add string mappings for these values (numbers 0-9) so that the keyboard shortcuts
            // will be displayed properly in menus.
            AddKey("0", Keys.D0);
            AddKey("1", Keys.D1);
            AddKey("2", Keys.D2);
            AddKey("3", Keys.D3);
            AddKey("4", Keys.D4);
            AddKey("5", Keys.D5);
            AddKey("6", Keys.D6);
            AddKey("7", Keys.D7);
            AddKey("8", Keys.D8);
            AddKey("9", Keys.D9);
        }

        /// <include file='doc\KeysConverter.uex' path='docs/doc[@for="KeysConverter.KeyNames"]/*' />
        /// <devdoc>
        ///  Access to a lookup table of name/value pairs for keys.  These are localized
        ///  names.
        /// </devdoc>
        private IDictionary KeyNames
        {
            get
            {
                if (keyNames == null)
                {
                    Initialize();
                }
                return keyNames;
            }
        }

        private List<string> DisplayOrder
        {
            get
            {
                if (displayOrder == null)
                {
                    Initialize();
                }
                return displayOrder;
            }
        }



        ///// <include file='doc\KeysConverter.uex' path='docs/doc[@for="KeysConverter.CanConvertFrom"]/*' />
        ///// <internalonly/>
        ///// <devdoc>
        /////    Determines if this converter can convert an object in the given source
        /////    type to the native type of the converter.
        ///// </devdoc>
        //public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        //{
        //    if (sourceType == typeof(string) || sourceType == typeof(Enum[]))
        //    {
        //        return true;
        //    }
        //    return base.CanConvertFrom(context, sourceType);
        //}

        ///// <include file='doc\EnumConverter.uex' path='docs/doc[@for="EnumConverter.CanConvertTo"]/*' />
        ///// <devdoc>
        /////    <para>Gets a value indicating whether this converter can
        /////       convert an object to the given destination type using the context.</para>
        ///// </devdoc>
        //public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        //{
        //    if (destinationType == typeof(Enum[]))
        //    {
        //        return true;
        //    }
        //    return base.CanConvertTo(context, destinationType);
        //}

        /// <include file='doc\KeysConverter.uex' path='docs/doc[@for="KeysConverter.Compare"]/*' />
        /// <devdoc>
        ///    <para>Compares two key values for equivalence.</para>
        /// </devdoc>
        public int Compare(object a, object b)
        {
            return String.Compare(ConvertToString(a), ConvertToString(b), false, CultureInfo.InvariantCulture);
        }


        public object ConvertFromString(String value)
        {

            string text = value.Trim();

            if (text.Length == 0)
            {
                return null;
            }
            else
            {

                // Parse an array of key tokens.
                //
                string[] tokens = text.Split(new char[] { '+' });
                for (int i = 0; i < tokens.Length; i++)
                {
                    tokens[i] = tokens[i].Trim();
                }

                // Now lookup each key token in our key hashtable.
                //
                Keys key = (Keys)0;
                bool foundKeyCode = false;

                for (int i = 0; i < tokens.Length; i++)
                {
                    object obj = KeyNames[tokens[i]];

                    if (obj == null)
                    {

                        // Key was not found in our table.  See if it is a valid value in
                        // the Keys enum.
                        //
                        obj = Enum.Parse(typeof(Keys), tokens[i], true);
                    }

                    if (obj != null)
                    {
                        Keys currentKey = (Keys)obj;

                        if ((currentKey & Keys.KeyCode) != 0)
                        {

                            // We found a match.  If we have previously found a
                            // key code, then check to see that this guy
                            // isn't a key code (it is illegal to have, say,
                            // "A + B"
                            //
                            if (foundKeyCode)
                            {
                                throw new FormatException("InvalidKeyCombination");
                            }
                            foundKeyCode = true;
                        }

                        // Now OR the key into our current key
                        //
                        key |= currentKey;
                    }
                    else
                    {

                        // We did not match this key.  Report this as an error too.
                        //
                        throw new FormatException("InvalidKeyName:" + tokens[i]);

                    }
                }

                return (object)key;
            }

        }

        public string ConvertToString(object value)
        {

            if (value is Keys || value is int)
            {
                Keys key = (Keys)value;
                bool added = false;
                ArrayList terms = new ArrayList();
                Keys modifiers = (key & Keys.Modifiers);

                // First, iterate through and do the modifiers. These are
                // additive, so we support things like Ctrl + Alt
                //
                for (int i = 0; i < DisplayOrder.Count; i++)
                {
                    string keyString = (string)DisplayOrder[i];
                    Keys keyValue = (Keys)keyNames[keyString];
                    if (((int)(keyValue) & (int)modifiers) != 0)
                    {
                        if (added)
                        {
                            terms.Add("+");
                        }

                        terms.Add((string)keyString);

                        added = true;
                    }
                }

                // Now reset and do the key values.  Here, we quit if
                // we find a match.
                //
                Keys keyOnly = (key & Keys.KeyCode);
                bool foundKey = false;

                if (added)
                {
                    terms.Add("+");
                }

                for (int i = 0; i < DisplayOrder.Count; i++)
                {
                    string keyString = (string)DisplayOrder[i];
                    Keys keyValue = (Keys)keyNames[keyString];
                    if (keyValue.Equals(keyOnly))
                    {
                        terms.Add((string)keyString);

                        added = true;
                        foundKey = true;
                        break;
                    }
                }

                // Finally, if the key wasn't in our list, add it to 
                // the end anyway.  Here we just pull the key value out
                // of the enum.
                //
                if (!foundKey && Enum.IsDefined(typeof(Keys), (int)keyOnly))
                {
                    terms.Add(((Enum)keyOnly).ToString());
                }

                StringBuilder b = new StringBuilder(32);
                foreach (string t in terms)
                {
                    b.Append(t);
                }
                return b.ToString();
            }
            else { return string.Empty; }
        }
    }

    //    /// <include file='doc\KeysConverter.uex' path='docs/doc[@for="KeysConverter.GetStandardValues"]/*' />
    //    /// <internalonly/>
    //    /// <devdoc>
    //    ///    Retrieves a collection containing a set of standard values
    //    ///    for the data type this validator is designed for.  This
    //    ///    will return null if the data type does not support a
    //    ///    standard set of values.
    //    /// </devdoc>
    //    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    //    {
    //        if (values == null)
    //        {
    //            ArrayList list = new ArrayList();

    //            ICollection keys = KeyNames.Values;

    //            foreach (object o in keys)
    //            {
    //                list.Add(o);
    //            }

    //            list.Sort(this);

    //            values = new StandardValuesCollection(list.ToArray());
    //        }
    //        return values;
    //    }

    //    /// <include file='doc\KeysConverter.uex' path='docs/doc[@for="KeysConverter.GetStandardValuesExclusive"]/*' />
    //    /// <internalonly/>
    //    /// <devdoc>
    //    ///    Determines if the list of standard values returned from
    //    ///    GetStandardValues is an exclusive list.  If the list
    //    ///    is exclusive, then no other values are valid, such as
    //    ///    in an enum data type.  If the list is not exclusive,
    //    ///    then there are other valid values besides the list of
    //    ///    standard values GetStandardValues provides.
    //    /// </devdoc>
    //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
    //    {
    //        return false;
    //    }

    //    /// <include file='doc\KeysConverter.uex' path='docs/doc[@for="KeysConverter.GetStandardValuesSupported"]/*' />
    //    /// <internalonly/>
    //    /// <devdoc>
    //    ///    Determines if this object supports a standard set of values
    //    ///    that can be picked from a list.
    //    /// </devdoc>
    //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //}

    ///// <devdoc>
    /////    <para>Represents a collection of values.</para>
    ///// </devdoc>
    //public class StandardValuesCollection : ICollection
    //{
    //    private ICollection values;
    //    private Array valueArray;

    //    /// <devdoc>
    //    ///    <para>
    //    ///       Initializes a new instance of the <see cref='System.ComponentModel.TypeConverter.StandardValuesCollection'/>
    //    ///       class.
    //    ///    </para>
    //    /// </devdoc>
    //    public StandardValuesCollection(ICollection values)
    //    {
    //        if (values == null)
    //        {
    //            values = new object[0];
    //        }

    //        Array a = values as Array;
    //        if (a != null)
    //        {
    //            valueArray = a;
    //        }

    //        this.values = values;
    //    }

    //    /// <devdoc>
    //    ///    <para>
    //    ///       Gets the number of objects in the collection.
    //    ///    </para>
    //    /// </devdoc>
    //    public int Count
    //    {
    //        get
    //        {
    //            if (valueArray != null)
    //            {
    //                return valueArray.Length;
    //            }
    //            else
    //            {
    //                return values.Count;
    //            }
    //        }
    //    }

    //    /// <devdoc>
    //    ///    <para>Gets the object at the specified index number.</para>
    //    /// </devdoc>
    //    public object this[int index]
    //    {
    //        get
    //        {
    //            if (valueArray != null)
    //            {
    //                return valueArray.GetValue(index);
    //            }
    //            IList list = values as IList;
    //            if (list != null)
    //            {
    //                return list[index];
    //            }
    //            // No other choice but to enumerate the collection.
    //            //
    //            valueArray = new object[values.Count];
    //            values.CopyTo(valueArray, 0);
    //            return valueArray.GetValue(index);
    //        }
    //    }

    //    /// <devdoc>
    //    ///    <para>Copies the contents of this collection to an array.</para>
    //    /// </devdoc>
    //    public void CopyTo(Array array, int index)
    //    {
    //        values.CopyTo(array, index);
    //    }

    //    /// <devdoc>
    //    ///    <para>
    //    ///       Gets an enumerator for this collection.
    //    ///    </para>
    //    /// </devdoc>
    //    public IEnumerator GetEnumerator()
    //    {
    //        return values.GetEnumerator();
    //    }

    //    /// <internalonly/>
    //    /// <devdoc>
    //    /// Retrieves the count of objects in the collection.
    //    /// </devdoc>
    //    int ICollection.Count
    //    {
    //        get
    //        {
    //            return Count;
    //        }
    //    }

    //    /// <internalonly/>
    //    /// <devdoc>
    //    /// Determines if this collection is synchronized.
    //    /// The ValidatorCollection is not synchronized for
    //    /// speed.  Also, since it is read-only, there is
    //    /// no need to synchronize it.
    //    /// </devdoc>
    //    bool ICollection.IsSynchronized
    //    {
    //        get
    //        {
    //            return false;
    //        }
    //    }

    //    /// <internalonly/>
    //    /// <devdoc>
    //    /// Retrieves the synchronization root for this
    //    /// collection.  Because we are not synchronized,
    //    /// this returns null.
    //    /// </devdoc>
    //    object ICollection.SyncRoot
    //    {
    //        get
    //        {
    //            return null;
    //        }
    //    }

    //    /// <internalonly/>
    //    /// <devdoc>
    //    /// Copies the contents of this collection to an array.
    //    /// </devdoc>
    //    void ICollection.CopyTo(Array array, int index)
    //    {
    //        CopyTo(array, index);
    //    }

    //    /// <internalonly/>
    //    /// <devdoc>
    //    /// Retrieves a new enumerator that can be used to
    //    /// iterate over the values in this collection.
    //    /// </devdoc>
    //    IEnumerator IEnumerable.GetEnumerator()
    //    {
    //        return GetEnumerator();
    //    }
    //}
}
