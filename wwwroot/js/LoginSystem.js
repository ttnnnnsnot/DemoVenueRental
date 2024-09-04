const loginedSystem = (isLoggedIn, isShowLoginModel) => {
    const roleAdminPages = ['/home/PlaceEdit'].map(page => page.toLowerCase()); // 需要權限的頁面路徑
    const loginPages = ['/home/PlaceManage', ...roleAdminPages].map(page => page.toLowerCase()); // 需要登入的頁面路徑

    // 檢查當下頁面是否為需要登入的頁面
    const isLoginPage = (pathName) => {
        return loginPages.includes(trimString(pathName));
    }

    const isRoleAdminPage = (pathName) => {
        return roleAdminPages.includes(trimString(pathName));
    }

    const onMounted = async () => {
        checkPathName();
        await listenerLink();
    }

    const listenerOnLogout = async () => {
        const loginModal = document.getElementById('loginModal');
        if (loginModal) {
            const onModalHidden = async function (event) {
                if (!isLoggedIn.value) {
                    checkPathName("/");
                }
                loginModal.removeEventListener('hidden.bs.modal', onModalHidden);
            };

            loginModal.addEventListener('hidden.bs.modal', onModalHidden);
        }
    }

    const checkPathName = async (pathName) => {
        if (pathName === "#")
            return;
        else if (pathName === "/")
            window.location.href = pathName;
        else if (!pathName) {
            // 如果沒有傳入pathName，則取得當前頁面路徑
            pathName = window.location.pathname.toLowerCase();
        } else {
            pathName = new URL(pathName, window.location.origin).pathname.toLowerCase();
        }

        // 如果是登入頁面，且未登入，則阻止默認事件，重新啟動登入頁面 
        if (isLoginPage(pathName) && !isLoggedIn.value) {
            listenerOnLogout();
            isShowLoginModel.value = true;
        } else if (isRoleAdminPage(pathName) && !await getRole("User")) {
            Alert.addDanger('您沒有權限訪問此頁面');
        } else if (pathName != window.location.pathname.toLowerCase()) {
            window.location.href = window.location.origin + pathName;
        }
    }

    // 監聽頁面連結
    const listenerLink = async () => {
        document.querySelectorAll('a').forEach(link => {
            link.addEventListener('click', async (event) => {
                event.preventDefault();
                const href = link.getAttribute('href');
                await checkPathName(href);
            });
        });
    }

    return {
        checkPathName,
        onMounted
    }
}

export default loginedSystem;