export interface ICase {
    id: number,
    name: string
}

export interface IUserType {
    id: number,
    isAdult: boolean,
    name: string
}

export interface IUserData {
    parentName?: string
    phone: string
    email: string,
    userType: IUserType
    firstName: string,
    lastName: string,
    patronymic: string,
    city: string,
    institution: string,
    grade: number,
    speciality?: string,
    case: ICase,
    consentId: string,
    solutionId: string,
    score?: number
    status: IUserStatus
}

export interface IUserStatus{
    id: number,
    name: string,
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
    video: string,
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