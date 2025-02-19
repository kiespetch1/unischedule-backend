using UniSchedule.Extensions.Attributes;

namespace UniSchedule.Identity.Shared;

public enum ActionOption
{
    [HandbookValue("3e10e9b9-b5c2-4814-ae5e-72f3fa3d78f2", "Создание")]
    Create,

    [HandbookValue("f09e86df-5a39-4c22-90e8-2b79ab16870c", "Чтение")]
    Read,

    [HandbookValue("00b7f04c-c63f-46df-9b01-b0c157b1092a", "Изменение")]
    Update,

    [HandbookValue("78f77b38-a23c-4f84-8383-08700ccba4e1", "Удаление")]
    Delete
}