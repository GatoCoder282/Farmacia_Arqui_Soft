namespace Farmacia_Arqui_Soft.Modules.ClientService.Application.Dtos
{
    
    public record ClientCreateDto(
       string FirstName,
       string LastName,
       string email,
       string nit
   );

    public record ClientUpdateDto(
       string FirstName,
       string LastName,
       string email,
       string nit
   );

}