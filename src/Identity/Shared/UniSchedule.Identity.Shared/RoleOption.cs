using UniSchedule.Extensions.Attributes;

namespace UniSchedule.Identity.Shared;

public enum RoleOption
{
    [HandbookValue("96ff0187-968e-4735-ac49-ad4353af3e77", "Пользователь")] 
    User,
    
    [HandbookValue("e4021903-c15b-400d-ba13-13c8b1fe0ccf", "Администратор")] 
    Admin,
    
    [HandbookValue("c58e5c66-5857-4cc3-b242-abb8bca1be0c", "Староста группы")] 
    GroupLeader,
    
    [HandbookValue("1d5ea95d-dd51-4d93-9e07-eb8a81f69545", "Работник ВУЗа")]
    Staff
}