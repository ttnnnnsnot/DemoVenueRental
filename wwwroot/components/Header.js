const headerOption = () => {
    const performShowRegisterModel = inject('showRegisterModel');
    const performShowLoginModel = inject('showLoginModel');
    const performLogout = inject('logouted');
    const currentState = inject('currentState');

    const showLogin = () => performShowLoginModel();
    const showRegister = () => performShowRegisterModel();
    const logout = () => performLogout();

    const headerLinks = reactive([
        {
            OrderNum: 1,state: [1,2], linktype: 'a', text: '找場地', link: `${getBaseUrl()}home/placeedit`, className: 'nav-link text-dark', show: false
        },
        {
            OrderNum: 2, state: [1,2], linktype: 'a', text: '成為場地主', link: `${getBaseUrl()}home/PlaceManage`, className: 'nav-link text-dark', show: false
        },
        {
            OrderNum: 3, state: [1], linktype: 'a', text: '註冊', className: 'nav-link text-dark', show: false, onclick: showRegister
        },
        {
            OrderNum: 4, state: [1], linktype: 'a', text: '登入', className: 'nav-link text-dark', show: false, onclick: showLogin
        },
        {
            OrderNum: 99, state: [2, 3], linktype: 'dropdown', show: false, onclick: logout
        },
        {
            OrderNum: 5, state: [3], linktype: 'a', text: '返回管理頁', className: 'nav-link text-dark', show: false, link: `${getBaseUrl()}home/PlaceManage`
        },
    ]);

    const setShowType = (state) => {
        headerLinks.forEach(item => {
            if (item.state.includes(state)) {
                item.show = true;
            } else {
                item.show = false;
            }
        });
        // 根據 OrderNum 進行排序
        headerLinks.sort((a, b) => a.OrderNum - b.OrderNum);
    };

    // 監聽currentState的變化
    watch(currentState, (newState) => {
        setShowType(newState);
    });

    return {
        headerLinks,
    };
};

const headerTemplate = defineAsyncComponent(async () => {
    return {
        template: await API.GetTemplate('templates/HeaderLinks.html'),
        setup() {
            const {
                headerLinks
            } = headerOption();

            return {
                headerLinks
            };
        }
    }

});

export default headerTemplate;