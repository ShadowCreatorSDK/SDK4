/// <summary>
/// The child object in the list sorted by what way
/// </summary>

public enum ListSortType
{
    /// <summary>
    /// Order by the base class of objects
    /// </summary>
    Childindex = 0,
    /// <summary>
    /// According to the object name alphabetical order
    /// </summary>
    ChildAlphabet,
    /// <summary>
    /// By the base class of objects reverse ordering sequence
    /// </summary>
    ChildIndexReverse,
    /// <summary>
    /// According to the child object name letters reverse order
    /// </summary>
    ChildAlphabetReverse
}
