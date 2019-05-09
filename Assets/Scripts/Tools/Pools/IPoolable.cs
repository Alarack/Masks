

public interface IPoolable {

    //string ObjectName { get; set; }
    //bool IsUsing { get; set; }
    void OnGet();
    void OnReturn();
}
