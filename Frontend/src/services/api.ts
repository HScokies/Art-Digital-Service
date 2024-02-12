import axios from 'axios';
import { IData, orderBy, param } from "src/components/dataGridView/interfaces"
import { IParticipantStatus, IUserData } from "src/interfaces"



export class API{
    public static readonly URL = new URL(import.meta.env.VITE_BACKEND_URL || "http://localhost:8080")

    private static api = axios.create({
        withCredentials: true,
        validateStatus: () => true
    });

    static emailExists = async (email:string) => {
        const url = new URL("users", API.URL)
        url.searchParams.append("email", email);
        return await this.api.get(url.toString());
    }

    static getParticipantTypes = async () => {
        const url = new URL("participants/types", API.URL)
        return await this.api.get(url.toString());
    }

    static register = async (data:FormData) => {
        const url = new URL("authentication/register", API.URL) 
        return await this.api.post(url.toString(), data);
    }

    static login = async (data:FormData) => {
        const url = new URL("authentication/login", API.URL)
        return await this.api.post(url.toString(), data)
    }

    static getCases = async(search?: string) => {
        const url = new URL("cases", API.URL);
        if (search)
            url.searchParams.append("search", search);
        return await this.api.get(url.toString());
    }
    
    static getCities = async () => {
        const url = new URL("utils/cities", API.URL);
        return await this.api.get(url.toString())
    }
    //#region Users dashboard
    static getUsers = async(offset:number, take:number, filters: param[], search?: string, orderBy?: orderBy): Promise<IData> => {
        const url = new URL("participants", API.URL);
        url.searchParams.append("offset", String(offset))
        url.searchParams.append("take", String(take))
        if (search){
            url.searchParams.append("search", search)
        }
        if (orderBy){
            url.searchParams.append("orderBy",orderBy.column)
            url.searchParams.append("asc",(orderBy.asc as unknown) as string)
        }

        filters.forEach((f) => {
            url.searchParams.append(f.name, String(f.value))
        })
        
        var response = await this.api.get(url.toString())
        return response.data;
    }

    static exportParticipants = (ids?: Set<number>): string => {
        const url = new URL("participants/export", API.URL);
        ids?.forEach((id) => {
            url.searchParams.append("id", String(id))
        })
        return url.toString();
    }

    static createUser = async(data: FormData) => {
        const url = new URL("participants", API.URL)
        const response = await this.api.post(url.toString(), data)
        if (response.status != 201){
            alert("Произошла ошибка! Проверьте консоль браузера для деталей");
            console.error(response)
            return false;
        } 
        alert("Участник успешно создан!")
        return true
    }

    static getUser = async(id: number): Promise<IUserData | undefined> => {
        const url = new URL(`participants/${id}`, API.URL)
        const response =  await this.api.get(url.toString());
        if (response.status != 200){
            alert("Произошла ошибка! Проверьте консоль браузера для деталей");
            console.error(response)
        } else return response.data;
    }

    static deleteUsers = async (ids?: Set<number>) => {
        const url = new URL("participants", API.URL);
        ids?.forEach((id) => {
            url.searchParams.append("id", String(id))
        })
        await this.api.delete(url.toString());
    }

    static updateUser = async(id: number, data: FormData) => {
        const url = new URL(`participants/${id}`, API.URL);        
        const response = await this.api.put(url.toString(), data);
        if (response.status != 204){
            alert("Произошла ошибка! Проверьте консоль браузера для деталей");
            console.error(response)
        } else alert("Участник успешно обновлен!")
    }

    static rateUser = async(id: number, data: FormData) => {
        const url = new URL(`participants/${id}`, API.URL);
        const response = await this.api.patch(url.toString(), data);
        if (response.status != 204){
            alert("Произошла ошибка! Проверьте консоль браузера для деталей");
            console.error(response)
        } else alert("Участник успешно обновлен!")
    } 

    static getUserStatuses = async(): Promise<IParticipantStatus[]> => {
        const url = new URL("participants/statuses", API.URL);

        return (await this.api.get(url.toString())).data;
    }

    static getParticipantStatuses = async(): Promise<IParticipantStatus[]> => {
        const url = new URL("participants/statuses-participant", API.URL);

        return (await this.api.get(url.toString())).data;
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
    
    //#region Staff dashboard
    static getStaffRoles = async() => {
        const url = new URL("staff/roles", API.URL);
        return await this.api.get(url.toString());
    }

    static getStaff =async (offset:number, take:number, filters: param[], search?: string, orderBy?: orderBy) => {
        const url = new URL("staff", API.URL);
        url.searchParams.append("offset", String(offset))
        url.searchParams.append("take", String(take))
        if (search){
            url.searchParams.append("search", search)
        }
        if (orderBy){
            url.searchParams.append("orderBy",orderBy.column)
            url.searchParams.append("asc",(orderBy.asc as unknown) as string)
        }

        filters.forEach((f) => {
            url.searchParams.append(f.name, String(f.value))
        })
        var response = await this.api.get(url.toString())
        return response.data;
    }

    static createStaff = async(data: FormData) => {
        const url = new URL("staff", API.URL)
        const response = await this.api.post(url.toString(), data)
        if (response.status != 201){
            alert("Произошла ошибка! Проверьте консоль браузера для деталей");
            console.error(response)
            return false
        } 
        alert("Сотрудник успешно создан!")
        return true;
    }

    static getStaffById = async(id: number) => {
        const url = new URL(`staff/${id}`, API.URL)
        const response =  await this.api.get(url.toString());
        if (response.status != 200){
            alert("Произошла ошибка! Проверьте консоль браузера для деталей");
            console.error(response)
        } else return response.data;
    }

    static updateStaff = async (id: number, data: FormData) => {
        const url = new URL(`staff/${id}`, API.URL)
        const response = await this.api.put(url.toString(), data);
        if (response.status != 204){
            alert("Произошла ошибка! Проверьте консоль браузера для деталей");
            console.error(response)
        } else alert("Сотрудник успешно обновлен!")
    }

    static deleteStaff = async(ids?: Set<number>) => {
        const url = new URL("staff", API.URL)
        ids?.forEach((id) => {
            url.searchParams.append("id", String(id))
        })
        await this.api.delete(url.toString());
    }

    static exportStaff = (ids?: Set<number>): string => {
        const url = new URL("staff/export", API.URL);
        ids?.forEach((id) => {
            url.searchParams.append("id", String(id))
        })
        return url.toString();
    }
    //#endregion
    static getPremissions = async() => {
        const url = new URL("staff/permissions", API.URL)
        return await this.api.get(url.toString())
    }
    
    
    static logout = async() => {
        const url = new URL("authentication/logout", API.URL);
        
        const response = await this.api.get(url.toString());
        return response;
    }

    static refreshToken = async() => {
        const url = new URL("authentication/refresh", API.URL)
        const response = await this.api.get(url.toString());
        if (response.status == 200) return response.data;
        return undefined 
    }

    static forgotPassword =async (data:FormData) => {
        const url = new URL(`authentication/forgot-password`, API.URL)
        return await this.api.post(url.toString(), data)
    }

    static resetPassword = async (token: string, data:FormData) => {
        const url = new URL(`authentication/reset/${token}`, API.URL)
        return await this.api.post(url.toString(), data)
    } 

    static checkIfAdult = async() => {
        const url = new URL("participants/isAdult", API.URL)
        return await this.api.get(url.toString())
    }

    static appendPersonalData = async(data: FormData) => {
        const url = new URL("participants/append-data", API.URL)
        return await this.api.patch(url.toString(), data)
    }

    static appendFiles = async(data: FormData) => {
        const url = new URL("participants/append-files", API.URL)
        return await this.api.patch(url.toString(), data)
    }

    static getProfile = async() => {
        const url = new URL("participants/profile", API.URL)
        return await this.api.get(url.toString());
    }

    static appendLegalFiles = async(data: FormData) =>{
        const url = new URL("files/legal", API.URL)
        const response = await this.api.patch(url.toString(), data)
        if (response.status != 204) {
            alert("Произошла ошибка! Проверьте консоль браузера для деталей");
            console.error(response)
        } else alert("Файлы успешно обновлены!")
    }

    static getCertificateConfig = async() =>{
        const url = new URL("files/certificate-config", API.URL);
        return this.api.get(url.toString())

    }
    static appendCertificateSettings = async(data: FormData) =>{
        const url = new URL("files/certificate", API.URL);
        const response = await this.api.patch(url.toString(), data)
        if (response.status != 204) {
            alert("Произошла ошибка! Проверьте консоль браузера для деталей");
            console.error(response)
        } else alert("Конфигурация сертификата успешно обновлена!")
    }
}