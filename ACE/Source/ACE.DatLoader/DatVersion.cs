namespace ACE.DatLoader
{
    public enum DatVersionType : uint
    {
        ERROR = 0,

        // From TOD through to end of retail
        TOD = 1,

        // From Launch up until the TOD patch
        DM = 2,

        // These two may not be required
        BETA = 3, 
        PREVIEW = 4
    }
}
