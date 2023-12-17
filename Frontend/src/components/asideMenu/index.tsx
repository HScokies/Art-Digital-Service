import { useState } from 'react'
import './style.scss'
import Logo from 'images/logo.webp'
import Icons from 'images/icons.svg'

interface props {
    items: menuElement[]
}

export interface menuElement {
    iconId: string,
    title: string,
    onClick: () => void
}

const AsideMenu = ({ items }: props) => {
    const handleLogout = () => {
        alert("logout!")
    }
    return (
        <aside className='aside'>
            <div className="aside_menu">
                <img className='aside_menu-logo' draggable={false} src={Logo} alt='logo' />
                {
                    items.map((e, i) => (
                        <span key={i} className='aside_menu-element' title={e.title} onClick={() => e.onClick()}>
                            <svg>
                                <use xlinkHref={Icons + e.iconId} />
                            </svg>
                        </span>
                    ))
                }
                <span className='aside_menu-element' title='Выйти' id='logout' onClick={() => handleLogout()}>
                    <svg>
                        <use xlinkHref={Icons + "#logout"} />
                    </svg>
                </span>
            </div>
        </aside>
    )
}
export default AsideMenu