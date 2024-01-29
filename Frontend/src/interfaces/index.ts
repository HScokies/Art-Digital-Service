export interface ICase {
    id: number,
    name: string
}

export interface IUserType {
    id: number,
    name: string
    isAdult: boolean,    
}

export interface ICity{
    id: number,
    name: string
}

export interface IParticipantStatus{
    id: string,
    name: string    
}

export interface IUserData {
    parentName?: string
    phone: string
    email: string,
    typeId: number,
    firstName: string,
    lastName: string,
    patronymic: string,
    city: string,
    institution: string,
    grade: number,
    speciality?: string,
    caseId: number,
    consentFilename: string,
    solutionFilename: string,
    rating?: number
    status: string
}

export interface IStaffData{
    email: string,
    firstName: string,
    lastName: string,
    patronymic: string,
    roleId: number
}


export interface IPermissions{
    readUsers: boolean
    createUsers: boolean,
    updateUsers: boolean,
    rateUsers: boolean,
    deleteUsers: boolean,

    readStaff: boolean,
    createStaff: boolean,
    updateStaff: boolean,
    deleteStaff: boolean,
    
    readCases: boolean,
    createCases: boolean,
    updateCases: boolean,
    deleteCases: boolean
}


export interface IProfileData{
    case: ICaseData,
    user: IUserInfo,
    legal: ILegalInfo
}

export interface ICaseData{
    name: string,
    task: string,
    youtubeId: string,
    stages: string[],
    criterias: string[]
}

interface IUserInfo{
    firstName: string,
    lastName: string,
    email: string,
    status?: IProfileStatus
}

export default interface IProfileStatus{
    name: string,
    file?: string
}

export interface IRole{
    id: number,
    name: string
}

interface ILegalInfo{
    phone: string,
    email: string,
    address: string,
    regulations: string,
    privacyPolicy: string,
    processingConsent: string
}