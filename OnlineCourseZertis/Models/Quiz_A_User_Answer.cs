//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnlineCourseZertis.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Quiz_A_User_Answer
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int QuizId { get; set; }
        public int Answer { get; set; }
        public string AnswerOther { get; set; }
    
        public virtual Quiz_Content Quiz_Content { get; set; }
        public virtual UserInfo UserInfo { get; set; }
    }
}