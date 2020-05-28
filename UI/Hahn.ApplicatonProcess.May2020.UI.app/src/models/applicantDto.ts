/**
 * An applicant
 */
export interface ApplicantDto { 
    /**
     * The id of applicant
     */
    id?: number;
    /**
     * The name of applicant
     */
    name?: string;
    /**
     * The family name of applicant
     */
    familyName?: string;
    /**
     * The address of applicant
     */
    address?: string;
    /**
     * The countryoforigin of applicant
     */
    countryOfOrigin?: string;
    /**
     * The emailaddress of applicant
     */
    emailAddress?: string;
    /**
     * The age of applicant
     */
    age?: number;
    /**
     * The property specifying if the applicant is hired
     */
    hired?: boolean;
}
