using document_editor.Controller;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace document_editor_test;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        var hey = new ExampleController();
        var zwei = await hey.GetAll();
        var uhewf = zwei as OkObjectResult;
        uhewf.Should().NotBeNull();
        uhewf.Value.Should().Be("Hello World!");
    }
}