namespace Farmacia_Arqui_Soft.Application.DTOs
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