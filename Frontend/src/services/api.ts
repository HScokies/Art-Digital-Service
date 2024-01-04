import { IData, orderBy, param } from "src/components/dataGridView/interfaces"
import UsersMock from './mock/usersMock.json'
import UserTypesMock from './mock/userTypesMock.json'
import CasesMock from './mock/casesMock.json'
import UserMock from './mock/userMock/A_userMock.json'
import UserStatusesMock from './mock/userStatusMock.json'
import PremissionsMock from './mock/PremissionsMock.json'
import CaseMock from './mock/caseMock.json'
import ProfileMock from './mock/userProfileMock.json'

import { ICaseData, IProfileData, IUserData, IUserStatus } from "src/interfaces"

export class API{
    static protocol: "http" | "https" = "http"
    static baseURL = "example.com"
    private static readonly URL = new URL(`${this.protocol}://${this.baseURL}/`)

    // offset take
    static getUsers = (offset:number, take:number, filters: param[], search?: string, orderBy?: orderBy): IData => {
        const getUsersURL = new URL(API.URL);
        getUsersURL.searchParams.append("offset", String(offset))
        getUsersURL.searchParams.append("take", String(take))
        if (search){
            getUsersURL.searchParams.append("search", search)
        }
        if (orderBy){
            getUsersURL.searchParams.append("orderBy",orderBy.column)
            getUsersURL.searchParams.append("order",orderBy.order)
        }

        filters.forEach((f) => {
            getUsersURL.searchParams.append(f.name, String(f.value))
        })
        console.debug(getUsersURL)
        return (UsersMock as unknown) as IData        
    }

    static exportUsers = (ids?: Set<number>) => {
        const exportUsersURL = new URL(API.URL);
        ids?.forEach((id) => {
            exportUsersURL.searchParams.append("id", String(id))
        })
        console.debug(exportUsersURL)
    }

    static deleteUsers = (ids: Set<number>) => {
        const deleteUserURL = new URL(API.URL);
        ids?.forEach((id) => {
            deleteUserURL.searchParams.append("id", String(id))
        })
        console.debug(deleteUserURL)
    }

    static getUserTypes = () => {
        return UserTypesMock
    }

    static getCases = () => {
        return CasesMock
    }

    static getUserStatuses = ():IUserStatus[] => {
        return UserStatusesMock
    }

    static createUser = (model: FormData) => {
        console.debug("CREATE USER", model)
    }

    static updateUser = (id: number, model: FormData) => {
        console.debug(`UPDATE USER id=${id}`, model)
    }

    static getUser = (id: number):IUserData => {
        return UserMock as IUserData
    }

    static getFile = (id: string) => {
        console.debug("filename:", id)
    }

    static register = (data: FormData) => {
        return true
    }
    
    static login = (credentials: FormData) => {
        return true
    }

    static logout = () => {

    }

    static refreshToken = () => {
        return true
    }

    static getPremissions = () => {
        return PremissionsMock
    }

    static upsertCase = (data: FormData, id?: number) => {
        return id? 'update' : 'insert'
    }
    static getCase = (id: number): ICaseData => {
        return CaseMock
    }

    static getProfileData = (): IProfileData => {
        return ProfileMock
    }
}