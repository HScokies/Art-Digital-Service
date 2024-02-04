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
    firstName: string,
    lastName: string,
    email: string,
    status?: IStatus,
    isAdult: boolean,
    case: ICaseData
}

export interface IStatus{
    text: string,
    download: boolean
}

export interface ICaseData{
    name: string,
    task: string,
    youtubeId: string,
    stages: string[],
    criterias: string[]
}

export default interface IProfileStatus{
    name: string,
    file?: string
}

export interface IRole{
    id: number,
    name: string
}