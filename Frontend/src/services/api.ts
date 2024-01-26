import axios, { AxiosError } from 'axios';
import { IData, orderBy, param } from "src/components/dataGridView/interfaces"
import UsersMock from './mock/usersMock.json'
import UserTypesMock from './mock/userTypesMock.json'
import CasesMock from './mock/casesMock.json'
import UserMock from './mock/userMock/A_userMock.json'
import UserStatusesMock from './mock/userStatusMock.json'
import PremissionsMock from './mock/PremissionsMock.json'
import CaseMock from './mock/caseMock.json'
import ProfileMock from './mock/userProfileMock.json'
import { ICaseData, IParticipantStatus, IProfileData, IUserData } from "src/interfaces"



export class API{
    static protocol: "http" | "https" = "https"
    static baseURL = "localhost:7220"
    public static readonly URL = new URL(`${this.protocol}://${this.baseURL}/`)
    
    private static api = axios.create({
        withCredentials: true,
        validateStatus: () => true
    });

    static trimData = (data: FormData) => {
        for (let pair of data.entries()){
            pair[1] = String(pair[1]).trim()
        }
        console.debug(data);
    }

    static emailExists = async (email:string) => {
        const url = new URL(API.URL + "users")
        url.searchParams.append("email", email);
        return await this.api.get(url.toString());
    }

    static getParticipantTypes = async () => {
        const url = new URL(API.URL + "participants/types")
        return await this.api.get(url.toString());
    }

    static register = async (data:FormData) => {
        const url = new URL(API.URL + "authentication/register")
        
        return await this.api.post(url.toString(), data);
    }

    static login = async (data:FormData) => {
        const url = new URL(API.URL + "authentication/login")

        return await this.api.post(url.toString(), data)
    }

    static getCases = async(search?: string) => {
        const url = new URL(API.URL + "cases");
        if (search)
            url.searchParams.append("search", search);

        return await this.api.get(url.toString());
    }
    
    //#region Users dashboard
    static getUsers = async(offset:number, take:number, filters: param[], search?: string, orderBy?: orderBy): Promise<IData> => {
        const getUsersURL = new URL(API.URL + "participants");
        getUsersURL.searchParams.append("offset", String(offset))
        getUsersURL.searchParams.append("take", String(take))
        if (search){
            getUsersURL.searchParams.append("search", search)
        }
        if (orderBy){
            getUsersURL.searchParams.append("orderBy",orderBy.column)
            getUsersURL.searchParams.append("asc",(orderBy.asc as unknown) as string)
        }

        filters.forEach((f) => {
            getUsersURL.searchParams.append(f.name, String(f.value))
        })
        
        var response = await this.api.get(getUsersURL.toString())
        return response.data;
    }

    static exportParticipants = (ids?: Set<number>): string => {
        const url = new URL(API.URL + "participants/export");
        ids?.forEach((id) => {
            url.searchParams.append("id", String(id))
        })
        return url.toString();
    }

    static createUser = async(model: FormData) => {
        const url = new URL(API.URL + "participants")
        const response = await this.api.post(url.toString(), model)
        if (response.status != 201){
            alert("Произошла ошибка! Проверьте консоль браузера для деталей");
            console.error(response)
        } else alert("Участник успешно создан!")
        
    }

    static getUser = async(id: number): Promise<IUserData | undefined> => {
        const url = new URL(API.URL + `participants/${id}`)
        const response =  await this.api.get(url.toString());
        if (response.status != 200){
            alert("Произошла ошибка! Проверьте консоль браузера для деталей");
            console.error(response)
        } else return response.data;
    }

    static deleteUsers = async (ids?: Set<number>) => {
        const url = new URL(API.URL + "participants");
        ids?.forEach((id) => {
            url.searchParams.append("id", String(id))
        })
        await this.api.delete(url.toString());
    }

    static updateUser = async(id: number, model: FormData) => {
        const url = new URL(API.URL + `participants/${id}`);
        
        const response = await this.api.put(url.toString(), model);
        if (response.status != 204){
            alert("Произошла ошибка! Проверьте консоль браузера для деталей");
            console.error(response)
        } else alert("Участник успешно обновлен!")
    }

    static getUserStatuses = async(): Promise<IParticipantStatus[]> => {
        const url = new URL(API.URL + "participants/statuses");

        return (await this.api.get(url.toString())).data;
    }

    static rateUser = async(id: number, model: FormData) => {
        const url = new URL(API.URL + `participants/${id}`);

        await this.api.patch(url.toString(), model);
    } 
    //#endregion

    //#region Cases dashboard
    static getCase =async (id:number) => {
        const url = new URL(API.URL + `cases/${id}`)
        
        return await this.api.get(url.toString());        
    }

    static updateCase = async(id: number, data: FormData) => {
        const url = new URL(API.URL + `cases/${id}`);
        var response = await this.api.put(url.toString(), data)
        if (response.status != 204){
            alert("Произошла ошибка! Проверьте консоль браузера для деталей");
            console.error(response)
        } else alert("Направление успешно обновлено !")
    }

    static createCase =async (data: FormData) => {
        const url = new URL(API.URL + "cases");
        var response = await this.api.post(url.toString(), data)
        if (response.status != 201){
            alert("Произошла ошибка! Проверьте консоль браузера для деталей");
            console.error(response)
        } else alert("Направление успешно создано !")
    }

    static dropCase =async (id: number) => {
        const url = new URL(API.URL + `cases/${id}`)
        const response = await this.api.delete(url.toString())
        if (response.status != 204){
            alert("Произошла ошибка! Проверьте консоль браузера для деталей");
            console.error(response)
        } else alert("Направление успешно удалено !")

    }
    //#endregion
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

    static getProfileData = ()=> {
        return ProfileMock
    }
}