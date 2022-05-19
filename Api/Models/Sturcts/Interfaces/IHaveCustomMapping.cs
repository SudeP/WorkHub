using AutoMapper;

namespace Api.Models.Structs
{
    public interface IHaveCustomMapping
	{
		void CreateMappings(Profile configuration);
	}
}