namespace ProductionSystem.Knowledge;

public enum FactKind
{
	Unknown = 0,
	Input,
	Intermediate,
	Target
}

public class Fact(string id, string code, FactKind kind, string description)
{
	public string		Id			{ get; set; }	= id;
	public string		Code		{ get; set; }	= code;
	public FactKind		Kind		{ get; set; }	= kind;
	public string		Description { get; set; }	= description;
	
	public override string ToString() => Description;
}
