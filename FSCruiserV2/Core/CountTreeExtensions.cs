namespace FSCruiser.Core
{
    public static class CountTreeExtensions
    {
        // internal static void SerializeCountSampleState(this CountTreeDO count)
        //{
        //    SampleSelecter selector = count.Tag as SampleSelecter;
        //    if (selector != null)
        //    {
        //        XmlSerializer serializer = new XmlSerializer(selector.GetType());
        //        StringWriter writer = new StringWriter();
        //        serializer.Serialize(writer, selector);
        //        count.SampleSelectorState = writer.ToString();
        //        count.SampleSelectorType = selector.GetType().Name;
        //        count.TreeCount = selector.Count;
        //    }
        //}

        // internal static SampleSelecter DeserializeCountSampleState(this CountTreeDO count)
        // {
        //     XmlSerializer serializer = null;

        //     switch (count.SampleSelectorType)
        //     {
        //         case "BlockSelecter":
        //             {
        //                 serializer = new XmlSerializer(typeof(BlockSelecter));
        //                 break;
        //             }
        //         case "SRSSelecter":
        //             {
        //                 serializer = new XmlSerializer(typeof(SRSSelecter));
        //                 break;
        //             }
        //         case "SystematicSelecter":
        //             {
        //                 serializer = new XmlSerializer(typeof(SystematicSelecter));
        //                 break;
        //             }

        //     }

        //     StringReader reader = new StringReader(count.SampleSelectorState);
        //     return (SampleSelecter) serializer.Deserialize(reader);
        // }
    }
}