namespace ProductionSystem;

public class Rule(string id, string conclusion, string explanation)
{
	public string Id				{ get; set; }	= id;
	public List<string> Conditions	{ get; }		= new List<string>();
	public string Conclusion		{ get; set; }	= conclusion;
	public string Explanation		{ get; set; }	= explanation;
}