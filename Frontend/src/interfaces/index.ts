export interface ICase {
    id: number,
    name: string
}

export interface IUserType {
    id: number,
    name: string
    isAdult: boolean,    
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


export interface IPermissions{
    users: IPermission,
    cases: IPermission,
    staff: IPermission
}

interface IPermission{
    create: boolean,
    read: boolean,
    update: boolean,
    delete: boolean
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

interface ILegalInfo{
    phone: string,
    email: string,
    address: string,
    regulations: string,
    privacyPolicy: string,
    processingConsent: string
}