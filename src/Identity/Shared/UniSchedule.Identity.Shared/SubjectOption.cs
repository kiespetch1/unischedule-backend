using UniSchedule.Extensions.Attributes;

namespace UniSchedule.Identity.Shared;

public enum SubjectOption
{
    [HandbookValue("e9eb88eb-9570-462a-bb0b-658f51ac9f00", "Пользователи")]
    Users,

    [HandbookValue("c0f29e79-f45c-459d-8de2-6b55724ad6e6", "Объявления")]
    Announcements,

    [HandbookValue("4bd397e6-5fe9-47dd-841e-739f83b5af9d", "Группы")]
    Groups,

    [HandbookValue("d23c8f85-8d10-4656-8c87-e8f56222a769", "Неделя")]
    Week,

    [HandbookValue("b0d4697e-bb0d-42f4-a946-096ca6583036", "День")]
    Day,

    [HandbookValue("4f86dd92-194d-42d4-b203-667d09cf82c2", "Пара")]
    Class
}