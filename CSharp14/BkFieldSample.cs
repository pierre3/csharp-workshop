namespace CSharp14;

class BkFieldSample 
{
    private string _name = "";
    public string Name { 
        get => _name.ToUpper();
        set => _name = value;
    }

    public string Name2
    {
        get => field.ToUpper();
        set;
    } = "";    
}

