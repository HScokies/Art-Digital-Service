import './style.scss'
import Icons from 'images/icons.svg'
import { useEffect, useState } from 'react'
import { Button, TextArea } from 'src/components'

interface props {
    _criterias?: string[]
}
const Criterias = ({ _criterias= [] }: props) => {
    const [criterias, setCriterias] = useState(_criterias)

    useEffect(() => {
        setCriterias(_criterias)
    }, [_criterias])
    
    const addStage = async () => {
        let copy = criterias
        await setCriterias([])
        copy.push('')
        setCriterias(copy)
    }

    const deleteStage = async(index: number) => {
        let copy = criterias
        await setCriterias([])
        copy.splice(index, 1)
        setCriterias(copy)
    }

    const setStage = async(index: number, text: string = '') => {
        setCriterias((criterias) => {
            criterias[index] = text
            return criterias
        })    
    }
    return (
        <>
            <div className='upsert-form-multiple header'>
                <h2>Критерии</h2>
                <Button isActive={true} type='button' clickHandler={addStage}>Добавить</Button>
            </div>
            <div className='upsert-form-multiple'>
                {
                    criterias.map((s, i) => (
                        <div key={i} className='upsert-form-multiple_element'>
                            <TextArea key={i} onBlur={(e) => setStage(i, e)} label={`Критерий №${i + 1}`} name='criteria' defaultValue={s} />
                            <svg onClick={() => deleteStage(i)}>
                                <use xlinkHref={Icons + '#trash'} />
                            </svg>
                        </div>
                    ))
                }
            </div>
        </>
    )
}
export default Criterias