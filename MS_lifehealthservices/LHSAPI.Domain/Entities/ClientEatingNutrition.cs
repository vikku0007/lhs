using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LHSAPI.Domain.Entities
{
   public class ClientEatingNutrition : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ClientId { get; set; }

        public bool? IsEatsIndependently { get; set; }

        public string EatingNutritionDetails { get; set; }

        public bool? IsPreparingMeals { get; set; }

        public string MealsDetails { get; set; }

        public bool? IsUsesUtensils { get; set; }

        public string UtensilsDetails { get; set; }

        public bool? IsFluids { get; set; }

        public string FluidsDetails { get; set; }

        public bool? IsModifiedFood { get; set; }

        public bool? IsPEG { get; set; }

        public bool? IsSwallowingImpairment { get; set; }

        public bool? IsDietPlan { get; set; }

        public string AllergiesDetails { get; set; }

        public bool? HasChoking { get; set; }

        public string ChokingDetails { get; set; }

        public string FoodPreferences { get; set; }
    }
}
