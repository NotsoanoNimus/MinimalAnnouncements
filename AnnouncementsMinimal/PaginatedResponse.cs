namespace AnnouncementsMinimal.Pager;

public sealed record ApiResponse<T>(
    uint Page,
    uint ItemsPerPage,
    T[]? Data,
    bool Success,
    bool Next,
    string Errors
);

public sealed class PaginatedResponse<T> {
    public const uint DEFAULT_ITEMS_PER_PAGE = 10;
    public const uint DEFAULT_PAGE = 0;


    // These parameters will be inserted into a template record.
    public uint Page { get; init; } = DEFAULT_PAGE;   // The default page to get when one is not specified in the ctor.
    public uint ItemsPerPage { get; init; } = DEFAULT_ITEMS_PER_PAGE;   // How many items should be returned per page.
    public T[]? Data { get; init; } = null;   // The data set to be paginated, returned from the persistent data store when a query is made.
    public bool Success { get; init; } = false;   // Indicates whether the pager successfully returned paged results.
    public bool Next { get; init; } = false;   // Whether another page follows the current page.
    public string Errors { get; init; } = "";   // Any error information is added to this string for unsuccessful requests.


    // Constructs a paginated HTTP response blob to parse.
    public PaginatedResponse(T[] responseData, uint? page, uint? itemsPerPage) {
        if(responseData == null || responseData.Length < 1) {
            this.Errors += "The resulting data set is empty.";
            return;
        }

        // Initialize the chosen page and count per page.
        page ??= DEFAULT_PAGE;
        itemsPerPage ??= DEFAULT_ITEMS_PER_PAGE;

        this.Page = (uint)page;
        this.ItemsPerPage = (uint)itemsPerPage;

        if(responseData.Length > itemsPerPage) {
            // Data must be paginated. First ensure the selected page is within a valid range to slice from the data array.
            if(responseData.Length < (page * itemsPerPage)) {
                this.Errors += $"The selected page is out of range. Valid pages are {DEFAULT_PAGE} through {(responseData.Length / itemsPerPage)}.";
                return;
            }

            // Get whether there is a Next page. This will also determine the array slicing behavior for the data set.
            this.Next = (responseData.Length > ((page + 1) * itemsPerPage));

            // Based on whether there is a next page, get the upper bound of the slice.
            int upperBound = this.Next
                ? (int)((page + 1) * itemsPerPage)
                : (responseData.Length)
            ;

            // Slice the data for the page.
            this.Data = responseData[(int)(page * itemsPerPage)..upperBound];

        } else {
            // No need to paginate. Requests only for the first (default) page are valid here when the request is paginated.
            if(page != DEFAULT_PAGE) {
                this.Errors += $"Only a single page of results exists. Either request all data at {itemsPerPage} items per page or request page {DEFAULT_PAGE} only at that limit.";
                return;
            }

            this.Data = responseData;
        }

        // If execution reaches this section, there are no errors.
        this.Success = true;
    }

    public ApiResponse<T> GetResponse() => new(this.Page, this.ItemsPerPage, this.Data, this.Success, this.Next, this.Errors);

}