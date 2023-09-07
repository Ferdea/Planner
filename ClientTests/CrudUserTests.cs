namespace ClientTests;

public class CrudUserTests
{
    private PlannerClient client;
    
    [SetUp]
    public void Setup()
    {
        client = new PlannerClient();
    }
}